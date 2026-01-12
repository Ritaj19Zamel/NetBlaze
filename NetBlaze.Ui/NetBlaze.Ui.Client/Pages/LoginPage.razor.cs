
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.JSInterop;
using MudBlazor;
using NetBlaze.SharedKernel.Dtos.Auth.Requests;
using NetBlaze.SharedKernel.Dtos.Fido.Requests;
using NetBlaze.SharedKernel.Enums;
using NetBlaze.SharedKernel.SharedResources;
using NetBlaze.Ui.Client.Services;
using NetBlaze.Ui.Client.Services.CommonServices;
using System.Text;
using System.Text.Json;

namespace NetBlaze.Ui.Client.Pages
{
    public partial class LoginPage : ComponentBase
    {
        [Inject] BlazeAuthService BlazeAuthService { get; set; } = default!;
        [Inject] BlazeFidoService BlazeFidoService { get; set; } = default!;
        [Inject] IJSRuntime JS { get; set; } = default!;
       // [Inject] ILocalStorageService LocalStorage { get; set; } = default!;
        [Inject] IJwtAuthService JwtAuthService { get; set; } = default!;
        [Inject] ISnackbar snackbar { get; set; } = default!;
        [Inject] NavigationManager Nav { get; set; } = default!;

        private MudForm _form;
        private LoginRequestDto _loginRequestDto = new();
        private bool _isProcessing;

        private bool _showPassword;


        private bool IsFormValid()
        {
            return !string.IsNullOrWhiteSpace(_loginRequestDto.Email)
                   && !string.IsNullOrWhiteSpace(_loginRequestDto.Password);
        }

        
        private async Task OnLoginAsync()
        {
            await _form.Validate();
            if (!_form.IsValid) return;

            _isProcessing = true;
            var response = await BlazeAuthService.LoginAsync(_loginRequestDto);
            _isProcessing = false;

            if (!response.Success)
            {
                snackbar.Add(response.Message, Severity.Error);
                return;
            }

            var token = response.Data!.Token;


            await JwtAuthService.SetTokenAsync(token);


            var roles = GetUserRolesFromToken(token);

            if (!roles.Contains(AppRoles.Employee))
            {
                NavigateAfterLogin("/home");
                return;
            }

            if (response.Data.RequiresDeviceRegistration)
            {
                if (!await StartFidoRegistration(response.Data.UserId))
                {
                    NavigateAfterLogin("/");
                    return;
                }
            }

            if (response.Data.RequiresDeviceAuthentication)
            {
                if(!await StartFidoLogin(response.Data.UserId))
                {
                    NavigateAfterLogin("/");
                    return;
                }
            }

            NavigateAfterLogin("/home");

        }


        private void TogglePassword()
        {
            _showPassword = !_showPassword;
        }

        private async Task<bool> StartFidoRegistration(long userId)
        {
            var optionsResponse = await BlazeFidoService.StartRegisterAsync(userId);

            if (!optionsResponse.Success)
            {
                snackbar.Add(
                    optionsResponse.Message ?? Messages.DeviceAlreadyRegistered,
                    Severity.Warning);

                return false;
            }


            var credential = await JS.InvokeAsync<object>(
                        "startFidoRegister",
                        optionsResponse.Data!.Options
                    );

            var dto = new FidoRegisterCompleteRequestDto
            {
                UserId = userId,
                AttestationJson = JsonSerializer.Serialize(credential)
            };

            var completeResponse = await BlazeFidoService.CompleteRegisterAsync(dto);


            if (completeResponse.Success)
            {
                snackbar.Add(completeResponse.Message, Severity.Success);
                return true;
            }
            else
            {
                snackbar.Add(completeResponse.Message, Severity.Error);
                return false;
            }
        }

        private async Task<bool> StartFidoLogin(long userId)
        {
            var optionsResponse = await BlazeFidoService.StartFidoLoginAsync(userId);

            if (!optionsResponse.Success)
            {
                snackbar.Add(optionsResponse.Message, Severity.Error);
                return false;
            }

            object assertion;

            try
            {
                assertion = await JS.InvokeAsync<object>(
                    "startFidoLogin",
                    optionsResponse.Data!.Options);
            }
            catch
            {
                snackbar.Add(Messages.DeviceAlreadyRegistered,
                    Severity.Warning);
                return false;
            }

            var dto = new FidoLoginCompleteRequestDto
            {
                UserId = userId,
                AttestationJson = JsonSerializer.Serialize(assertion)
            };

            var completeResponse = await BlazeFidoService.CompleteFidoLoginAsync(dto);

            if (!completeResponse.Success)
            {
                snackbar.Add(completeResponse.Message, Severity.Error);
                return false;
            }
            return true;
        }
        private List<AppRoles> GetUserRolesFromToken(string token)
        {
            var roles = new List<AppRoles>();

            var parts = token.Split('.');
            if (parts.Length != 3) return roles;

            var payload = parts[1];
            var paddedPayload = payload.PadRight(
                payload.Length + (4 - payload.Length % 4) % 4, '=');

            var jsonBytes = Convert.FromBase64String(paddedPayload);
            var jsonString = Encoding.UTF8.GetString(jsonBytes);

            var claims = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(jsonString);

            if (claims == null)
                return roles;

            // Check for "role" key (JWT standard) or full claim type URI
            JsonElement roleElement;
            if (claims.TryGetValue("role", out roleElement) || 
                claims.TryGetValue("http://schemas.microsoft.com/ws/2008/06/identity/claims/role", out roleElement))
            {
                if (roleElement.ValueKind == JsonValueKind.Array)
                {
                    foreach (var r in roleElement.EnumerateArray())
                    {
                        if (Enum.TryParse<AppRoles>(r.GetString(), out var role))
                            roles.Add(role);
                    }
                }
                else
                {
                    if (Enum.TryParse<AppRoles>(roleElement.GetString(), out var singleRole))
                        roles.Add(singleRole);
                }
            }

            return roles;
        }


        private void NavigateAfterLogin(string returnUrl)
        {
            var uri = Nav.ToAbsoluteUri(Nav.Uri);
            var query = QueryHelpers.ParseQuery(uri.Query);


            if (query.TryGetValue("returnUrl", out var url))
                returnUrl = url! + "?fromLogin=true";

            Nav.NavigateTo(returnUrl, replace: true);
        }





    }
}
