using Microsoft.AspNetCore.Components;
using MudBlazor;
using NetBlaze.SharedKernel.Dtos.Auth.Requests;
using NetBlaze.SharedKernel.Dtos.Department.Responses;
using NetBlaze.SharedKernel.Dtos.Role.Responses;
using NetBlaze.SharedKernel.Dtos.User.Responses;
using NetBlaze.Ui.Client.Services;

namespace NetBlaze.Ui.Client.Pages
{
    public partial class Register :ComponentBase
    {
        [Inject] BlazAuthService AuthService { get; set; } = default!;
        [Inject] BlazRoleService RoleService { get; set; } = default!;
        [Inject] BlazDepartmentService DepartmentService { get; set; } = default!;
        [Inject] BlazUserService UserService { get; set; } = default!;
        [Inject] ISnackbar snackbar { get; set; } = default!;

        private MudForm _form;
        private bool _loading;
        private RegisterRequestDto _model = new();

        private List<GetRoleResponseDto> _roles = new();
        private List<GetManagerResponseDto> _mangers = new();
        private List<GetDepartmentResponseDto> _departments = new();

        protected async override Task OnInitializedAsync()
        {
            await LoadLookupsAsync();
        }
        protected async Task LoadLookupsAsync() {
            
            var rolesResponse = await RoleService.GetAllAsync();

            if (rolesResponse.Success) {
                _roles = rolesResponse.Data ?? new List <GetRoleResponseDto>();
            }

            var mangersResponse = await UserService.GetManagersAsync();

            if (mangersResponse.Success)
            {
                _mangers = mangersResponse.Data  ??  new List<GetManagerResponseDto>();
            }

            var departmentResponse = await DepartmentService.GetAllAsync();

            if (departmentResponse.Success)
            {
                _departments = departmentResponse.Data ?? new List<GetDepartmentResponseDto>();
            }

    }
        private async Task RegisterAsync() { 

        await _form.Validate();
            if (!_form.IsValid)
                return;

            _loading = true;

            var response = await AuthService.RegisterAsync(_model);
            _loading = false;
            if (response.Success)
            {
                NavigationManager.NavigateTo("/login");

            }
            else {
                snackbar.Add(response.Message,Severity.Error);
            }
        }

    }
}
