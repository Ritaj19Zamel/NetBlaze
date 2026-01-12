using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using NetBlaze.SharedKernel.HelperUtilities.Constants;
using NetBlaze.Ui.Client.InternalHelperTypes.Constants;
using NetBlaze.Ui.Client.Services.CommonServices;
using System.Globalization;
using System.Text.Json;

namespace NetBlaze.Ui.Client.SharedRazor.Layouts
{
    public partial class MainLayout
    {
        [Inject] private ILocalStorageService LocalStorage { get; set; } = default!;
        [Inject] private NavigationManager Nav { get; set; } = default!;
        [Inject] private IJwtAuthService JwtAuthService { get; set; } = default!;
        [Inject] private AuthenticationStateProvider AuthStateProvider { get; set; } = default!;

        private MudTheme NetBlazeTheme => SharedTheme.NetBlazeTheme;

        private bool _rightToLeft =>
            CultureInfo.CurrentCulture.Name == LanguageCode.ARABIC_CODE;

        private bool _sidebarOpen = true;
        private string _userName = "User";
        private string _userEmail = "user@example.com";

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                // Initialize auth state from localStorage after first render
                await InitializeAuthStateAsync();
                await LoadUserInfoFromToken();
                StateHasChanged();
            }
        }

        private async Task InitializeAuthStateAsync()
        {
            try
            {
                var token = await LocalStorage.GetItemAsync<string>("auth_token");
                if (!string.IsNullOrWhiteSpace(token))
                {
                    // Update auth state with stored token
                    await ((JwtAuthStateProvider)JwtAuthService).SetTokenAsync(token);
                }
            }
            catch
            {
                // Token doesn't exist or is invalid
            }
        }

        private async Task LoadUserInfoFromToken()
        {
            try
            {
                var token = await LocalStorage.GetItemAsync<string>("auth_token");
                if (!string.IsNullOrWhiteSpace(token))
                {
                    var parts = token.Split('.');
                    if (parts.Length == 3)
                    {
                        var payload = parts[1];
                        var paddedPayload = payload.PadRight(payload.Length + (4 - payload.Length % 4) % 4, '=');
                        var jsonBytes = Convert.FromBase64String(paddedPayload);
                        var jsonString = System.Text.Encoding.UTF8.GetString(jsonBytes);
                        var claims = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(jsonString);

                        if (claims != null)
                        {
                            if (claims.ContainsKey("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"))
                                _userName = claims["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"].GetString() ?? "User";

                            if (claims.ContainsKey("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress"))
                                _userEmail = claims["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress"].GetString() ?? "user@example.com";
                        }
                    }
                }
            }
            catch
            {
                _userName = "User";
                _userEmail = "user@example.com";
            }
        }

        private void ToggleSidebar()
        {
            _sidebarOpen = !_sidebarOpen;
        }

        private Task OpenEditProfileDialogAsync()
        {
            Nav.NavigateTo("/profile/edit");
            return Task.CompletedTask;
        }

        private async Task OnLogoutAsync()
        {
            await JwtAuthService.ClearAsync();
            Nav.NavigateTo("/", forceLoad: true);
        }
    }
}