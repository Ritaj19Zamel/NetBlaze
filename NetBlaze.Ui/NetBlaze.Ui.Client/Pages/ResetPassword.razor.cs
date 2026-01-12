using Microsoft.AspNetCore.Components;
using MudBlazor;
using NetBlaze.SharedKernel.Dtos.Auth.Requests;
using NetBlaze.Ui.Client.Services;

namespace NetBlaze.Ui.Client.Pages
{
    public partial class ResetPassword
    {
        [Inject] BlazeAuthService BlazeAuthService { get; set; } = default!;
        [Inject] ISnackbar snackbar { get; set; } = default!;
        [Inject] NavigationManager Nav { get; set; } = default!;

        private MudForm _form;
        private ResetPasswordRequestDto _model = new();

        protected override void OnInitialized()
        {
            var uri = Nav.ToAbsoluteUri(Nav.Uri);
            var query = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(uri.Query);

            _model.Email = query["email"];
            _model.Token = query["token"];
        }

        private async Task ResetAsync()
        {
            await _form.Validate();
            if (!_form.IsValid) return;

            var response = await BlazeAuthService.ResetPasswordAsync(_model);

            if (response.Success)
            {
                snackbar.Add(response.Message, Severity.Success);
                Nav.NavigateTo("/");
            }
        }
    }
}
