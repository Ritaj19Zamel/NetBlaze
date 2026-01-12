using Microsoft.AspNetCore.Components;
using MudBlazor;
using NetBlaze.SharedKernel.Dtos.Department.Responses;
using NetBlaze.SharedKernel.Dtos.Role.Responses;
using NetBlaze.SharedKernel.Dtos.User.Requests;
using NetBlaze.SharedKernel.Dtos.User.Responses;
using NetBlaze.Ui.Client.Services;

namespace NetBlaze.Ui.Client.Dialogs.User
{
    public partial class UpdateUserDialog
    {
        [CascadingParameter]
        private IMudDialogInstance MudDialog { get; set; } = default!;

        [Parameter]
        public GetUserResponseDto GetUserResponseDto { get; set; } = null!;

        [Inject] BlazeUserService BlazeUserService { get; set; } = null!;
        [Inject] BlazeRoleService BlazeRoleService { get; set; } = null!;
        [Inject] BlazeDepartmentService BlazeDepartmentService { get; set; } = null!;

        private List<GetRoleResponseDto> Roles = [];
        private List<GetDepartmentResponseDto> Departments = [];
        private List<GetManagerResponseDto> Managers = [];

        protected override async Task OnInitializedAsync()
        {
            var rolesResponse = await BlazeRoleService.GetAllRolesAsync();
            var departmentsResponse = await BlazeDepartmentService.GetAllDepartmentAsync();
            var managersResponse = await BlazeUserService.GetManagersAsync();

            Roles = rolesResponse.Data ?? [];
            Departments = departmentsResponse.Data ?? [];
            Managers = managersResponse.Data ?? [];
        }

        private async Task SubmitAsync()
        {
            var model = new UpdateUserRequestDto
            {
                Id = GetUserResponseDto.Id,
                DisplayName = GetUserResponseDto.DisplayName,
                RoleId = GetUserResponseDto.RoleId,
                ManagerId = GetUserResponseDto.ManagerId,
                DepartmentId = GetUserResponseDto.DepartmentId
            };

            await BlazeUserService.UpdateUserAsync(model);
            MudDialog.Close(DialogResult.Ok(true));
        }

        private void Cancel() => MudDialog.Cancel();

        private string GetDepartmentName(long id) => Departments.FirstOrDefault(x => x.Id == id)?.Name ?? id.ToString();
        private string GetRoleName(long id) => Roles.FirstOrDefault(x => x.Id == id)?.RoleName ?? id.ToString();
    }
}
