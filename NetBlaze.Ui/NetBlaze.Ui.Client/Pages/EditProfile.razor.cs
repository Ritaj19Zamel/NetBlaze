using Microsoft.AspNetCore.Components;
using NetBlaze.SharedKernel.Dtos.User.Requests;
using NetBlaze.Ui.Client.Services;

namespace NetBlaze.Ui.Client.Pages
{
    public partial class EditProfile
    {
        [Inject] BlazeUserService BlazeUserService { get; set; } = default!;
        [Inject] NavigationManager Navigation { get; set; } = default!;

        private EditProfileRequestDto _model = new();

        private bool _isLoading = true;
        private bool _isSubmitting;
        private string? _errorMessage;

        private bool _showCurrentPassword;
        private bool _showNewPassword;
        private bool _showConfirmPassword;

        protected override async Task OnInitializedAsync()
        {
            await LoadProfileAsync();
        }

        private async Task LoadProfileAsync()
        {
            _isLoading = true;
            _errorMessage = null;

            var response = await BlazeUserService.GetCurrentUserProfileAsync();

            if (!response.Success || response.Data == null)
            {
                _errorMessage = response.Message;
                _isLoading = false;
                return;
            }

            _model = new EditProfileRequestDto
            {
                DisplayName = response.Data.DisplayName,
                Email = response.Data.Email,
                PhoneNumber = response.Data.PhoneNumber
            };

            _isLoading = false;
        }

        private async Task SubmitAsync()
        {
            if (_isSubmitting)
                return;

            _isSubmitting = true;
            _errorMessage = null;

            var response = await BlazeUserService.EditUserProfileAsync(_model);

            _isSubmitting = false;

            if (!response.Success)
            {
                _errorMessage = response.Message;
                return;
            }

            Navigation.NavigateTo("/home");
        }

        private void Cancel()
        {
            Navigation.NavigateTo("/home");
        }

        private bool IsFormValid()
        {
            return !string.IsNullOrWhiteSpace(_model.DisplayName) &&
                !string.IsNullOrWhiteSpace(_model.Email);
            
        }

        private void ToggleCurrentPassword()
            => _showCurrentPassword = !_showCurrentPassword;

        private void ToggleNewPassword()
            => _showNewPassword = !_showNewPassword;

        private void ToggleConfirmPassword()
            => _showConfirmPassword = !_showConfirmPassword;
    }
}
