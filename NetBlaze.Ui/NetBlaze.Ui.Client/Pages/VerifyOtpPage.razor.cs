using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;
using MudBlazor;
using NetBlaze.SharedKernel.Dtos.RandomCheck.Requests;
using NetBlaze.SharedKernel.SharedResources;
using NetBlaze.Ui.Client.Services;
using NetBlaze.Ui.Client.Services.CommonServices;

namespace NetBlaze.Ui.Client.Pages
{
    public partial class VerifyOtpPage
    {
        [Inject] BlazeRandomCheckService RandomCheckService { get; set; } = default!;
        [Inject] NavigationManager Nav { get; set; } = default!;
        [Inject] ILocalStorageService LocalStorage { get; set; } = default!;
        [Inject] AuthGuard AuthGuard { get; set; } = default!;
        [Inject] ISnackbar Snackbar { get; set; } = default!;

        private string _otp = string.Empty;


        private bool _checked;

        protected override async Task OnInitializedAsync()
        {
            var uri = Nav.ToAbsoluteUri(Nav.Uri);
            var query = QueryHelpers.ParseQuery(uri.Query);

            if (!query.TryGetValue("fromLogin", out var fromLogin)
               || fromLogin != "true")
            {
                Nav.NavigateTo("/?returnUrl=/verify-otp", replace: true);
            }


           
        }
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (!firstRender || _checked)
                return;

            _checked = true;

            var uri = Nav.ToAbsoluteUri(Nav.Uri);
            var query = QueryHelpers.ParseQuery(uri.Query);

            if (!query.TryGetValue("fromLogin", out var fromLogin)
                || fromLogin != "true")
            {
                Nav.NavigateTo("/?returnUrl=/verify-otp", replace: true);
            }
        }


        private async Task VerifyOtpAsync()
        {
            var response = await RandomCheckService.VerifyOtpAsync(
                new VerifyOTPRequestDto
                {
                    OTP = _otp
                });

            if (response.Success)
            {
                Snackbar.Add(Messages.OtpVerified, Severity.Success);
                Nav.NavigateTo("/home");
            }
            else
            {
                Snackbar.Add(response.Message, Severity.Error);
            }
        }
    }

}
