using Microsoft.AspNetCore.Components;
using MudBlazor;
using NetBlaze.SharedKernel.Dtos.Auth.Requests;
using NetBlaze.SharedKernel.Dtos.Department.Responses;
using NetBlaze.SharedKernel.Dtos.Role.Responses;
using NetBlaze.SharedKernel.Dtos.User.Responses;
using NetBlaze.Ui.Client.Services;

namespace NetBlaze.Ui.Client.Pages
{
    public partial class RegisterPage : ComponentBase
    {
        [Inject] BlazeAuthService BlazeAuthService { get; set; } = default!;
        [Inject] BlazeDepartmentService BlazeDepartmentService { get; set; } = default!;
        [Inject] BlazeRoleService BlazeRoleService { get; set; } = default!;
        [Inject] BlazeUserService BlazeUserService { get; set; } = default!;
        [Inject] ISnackbar snackbar { get; set; } = default!;
        [Inject] NavigationManager Nav { get; set; } = default!;

        private MudForm _form = default!;
        private RegisterRequestDto _registerRequestDto = new();
        private List<GetDepartmentResponseDto> _departments = new();
        private List<GetRoleResponseDto> _roles = new();
        private List<GetManagerResponseDto> _managers = new();

        private bool _showPassword;
        private bool _showConfirmPassword;
        private bool _isPageReady = false;
        private bool _isSubmitting = false;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await LoadLookupsAsync();
                _isPageReady = true;
                StateHasChanged();
            }
        }

        private string GetDepartmentName(long id) => _departments.FirstOrDefault(x => x.Id == id)?.Name ?? id.ToString();
        private string GetRoleName(long id) => _roles.FirstOrDefault(x => x.Id == id)?.RoleName ?? id.ToString();
        private string GetManagerName(long? id) => _managers.FirstOrDefault(x => x.Id == id)?.Name ?? (id?.ToString() ?? "");

        private async Task LoadLookupsAsync()
        {
            var departmentsTask = BlazeDepartmentService.GetAllDepartmentAsync();
            var rolesTask = BlazeRoleService.GetAllRolesAsync();
            var managersTask = BlazeUserService.GetManagersAsync();

            await Task.WhenAll(departmentsTask, rolesTask, managersTask);

            if (departmentsTask.Result.Success)
                _departments = departmentsTask.Result.Data ?? new();

            if (rolesTask.Result.Success)
                _roles = rolesTask.Result.Data ?? new();

            if (managersTask.Result.Success)
                _managers = managersTask.Result.Data ?? new();
        }

        private async Task OnRegisterAsync()
        {
            await _form.Validate();
            if (!_form.IsValid) return;

            _isSubmitting = true;
            var response = await BlazeAuthService.RegisterAsync(_registerRequestDto);
            _isSubmitting = false;

            if (response.Success)
            {
                snackbar.Add("User added successfully!", Severity.Success);

                await Task.Delay(1500);
                Nav.NavigateTo("/users");
            }
            else
            {
                snackbar.Add(response.Message ?? "Failed to add user", Severity.Error);
            }
        }

        private void ResetForm()
        {
            _registerRequestDto = new RegisterRequestDto();
            StateHasChanged();
        }

        private void TogglePassword()
        {
            _showPassword = !_showPassword;
        }

        private void ToggleConfirmPassword()
        {
            _showConfirmPassword = !_showConfirmPassword;
        }
    }
}