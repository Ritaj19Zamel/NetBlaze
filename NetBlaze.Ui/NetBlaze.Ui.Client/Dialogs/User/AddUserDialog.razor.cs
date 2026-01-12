using Microsoft.AspNetCore.Components;
using MudBlazor;
using NetBlaze.SharedKernel.Dtos.Auth.Requests;
using NetBlaze.SharedKernel.Dtos.Department.Responses;
using NetBlaze.SharedKernel.Dtos.Role.Responses;
using NetBlaze.SharedKernel.Dtos.User.Responses;
using NetBlaze.Ui.Client.Services;

namespace NetBlaze.Ui.Client.Dialogs.User
{
    public partial class AddUserDialog
    {
        [CascadingParameter] IMudDialogInstance MudDialog { get; set; } = null!;

        [Inject] BlazeAuthService BlazeAuthService { get; set; } = null!;
        [Inject] BlazeRoleService BlazeRoleService { get; set; } = null!;
        [Inject] BlazeDepartmentService BlazeDepartmentService { get; set; } = null!;
        [Inject] BlazeUserService BlazeUserService { get; set; } = null!;



        private RegisterRequestDto _model = new();
        private bool _isSubmitting;
        private bool _isLoadingData = true;
        private bool _showPassword;
        private bool _showConfirmPassword;

        private List<GetRoleResponseDto> _roles = [];
        private List<GetDepartmentResponseDto> _departments = [];
        private List<GetManagerResponseDto> _managers = [];

        private bool IsFormValid()
        {
            return !string.IsNullOrWhiteSpace(_model.DisplayName) &&
                          !string.IsNullOrWhiteSpace(_model.Email) &&
                          !string.IsNullOrWhiteSpace(_model.PhoneNumber) &&
                          !string.IsNullOrWhiteSpace(_model.Password) &&
                          !string.IsNullOrWhiteSpace(_model.ConfirmPassword) &&
                          _model.DepartmentId > 0 &&
                          _model.RoleId > 0;
        }
        private async Task LoadLookupsAsync()
        {
            StateHasChanged();
            var departmentsTask = BlazeDepartmentService.GetAllDepartmentAsync();
            var rolesTask = BlazeRoleService.GetAllRolesAsync();
            var managersTask = BlazeUserService.GetManagersAsync();

            await Task.WhenAll(departmentsTask, rolesTask, managersTask);

            if (departmentsTask.Result.Success)
            {
                _departments = departmentsTask.Result.Data ?? new();
            }

            if (rolesTask.Result.Success)
            {
                _roles = rolesTask.Result.Data ?? new();
            }

            if (managersTask.Result.Success)
            {
                _managers = managersTask.Result.Data ?? new();
            }
        }

        protected override async Task OnInitializedAsync()
        {
           await LoadLookupsAsync();
            _isLoadingData = false;
            StateHasChanged();
        }

        private async Task SubmitAsync()
        {
            if (_isSubmitting) return;

            _isSubmitting = true;
            var result = await BlazeAuthService.RegisterAsync(_model);
            _isSubmitting = false;

            if (result.Success)
            {
                MudDialog.Close(DialogResult.Ok(true));
            }
        }

        private void Cancel() => MudDialog.Cancel();

        private void TogglePassword() => _showPassword = !_showPassword;
        private void ToggleConfirmPassword() => _showConfirmPassword = !_showConfirmPassword;

        private string GetDepartmentName(long id) => _departments.FirstOrDefault(x => x.Id == id)?.Name ?? id.ToString();
        private string GetRoleName(long id) => _roles.FirstOrDefault(x => x.Id == id)?.RoleName ?? id.ToString();
        private string GetManagerName(long? id) => _managers.FirstOrDefault(x => x.Id == id)?.Name ?? (id?.ToString() ?? "");
    }
}