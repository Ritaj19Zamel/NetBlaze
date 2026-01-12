using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using MudBlazor;
using NetBlaze.SharedKernel.Dtos.Auth.Requests;
using NetBlaze.Ui.Client.Services;

namespace NetBlaze.Ui.Client.Pages
{
    public partial class ForgetPasswordPage
    {

        [Inject] BlazeAuthService BlazeAuthService { get; set; } = default!;
        [Inject] ISnackbar snackbar { get; set; } = default!;
        [Inject] NavigationManager Nav { get; set; } = default!;


        private MudForm _form;
        private ForgetPasswordRequestDto _model = new();


        private async Task SendAsync()
        {
            await _form.Validate();
            if (!_form.IsValid) return;

            _model.ClientUrl = $"{Nav.BaseUri}reset-password";

            var response = await BlazeAuthService.ForgetPasswordAsync(_model);

            if (response.Success)
            {
                snackbar.Add(response.Message, Severity.Success);
                Nav.NavigateTo("/");
            }
        }
    }
}
