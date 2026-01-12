using Microsoft.AspNetCore.Components;
using MudBlazor;
using NetBlaze.SharedKernel.Dtos.Department.Responses;
using NetBlaze.SharedKernel.SharedResources;
using NetBlaze.Ui.Client.Dialogs.Department;
using NetBlaze.Ui.Client.Services;

namespace NetBlaze.Ui.Client.Components.Concrete.Department
{
    public partial class DepartmentCrudOperationsComponent
    {
        [Inject] BlazeDepartmentService BlazeDepartmentService { get; set; } = default!;
        [Inject] IDialogService DialogService { get; set; } = default!;
        [Parameter] public GetDepartmentResponseDto GetDepartmentResponseDto { get; set; } = null!;

        private List<GetDepartmentResponseDto>? _departments;
        private bool _isLoading;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await LoadDepartmentsAsync();
                StateHasChanged();
            }
        }

        private async Task LoadDepartmentsAsync()
        {
            _isLoading = true;
            StateHasChanged();

            var response = await BlazeDepartmentService.GetAllDepartmentAsync();
            if (response.Success)
                _departments = response.Data;

            _isLoading = false;
        }

        private async Task OpenAddDialogAsync()
        {
            var parameters = new DialogParameters();

            var options = new DialogOptions
            {
                CloseOnEscapeKey = true,
                MaxWidth = MaxWidth.Large,
                FullWidth = true
            };

            var dialog = await DialogService.ShowAsync<AddDepartmentDialog>(Messages.AddDepartment, parameters, options);

            var result = await dialog.Result;

            if (!result.Canceled)
            {
                await LoadDepartmentsAsync();
            }
        }

        private async Task OpenUpdateDialogAsync(GetDepartmentResponseDto department)
        {
            var fullDepartmentResponse = await BlazeDepartmentService.GetDepartmentByIdAsync(department.Id);

            if (!fullDepartmentResponse.Success)
            {
                return;
            }

            var parameters = new DialogParameters
            {
                ["GetDepartmentResponseDto"] = department
            };


            var options = new DialogOptions
            {
                CloseOnEscapeKey = true,
                MaxWidth = MaxWidth.Large,
                FullWidth = true
            };

            var dialog = await DialogService.ShowAsync<UpdateDepartmentDialog>(
                Messages.EditDepartment,
                parameters,
                options);

            var result = await dialog.Result;

            if (!result.Canceled)
            {
                await LoadDepartmentsAsync();
            }
        }

        private async Task DeleteDepartmentAsync(long id)
        {
            var confirm = await DialogService.ShowMessageBox(
                Messages.DeleteDepartment,
                Messages.ConfirmDelete,
                yesText: Messages.DeleteDepartment,
                cancelText: Messages.Cancel);

            if (confirm == true)
            {
                await BlazeDepartmentService.DeleteDepartmentAsync(id);
                await LoadDepartmentsAsync();
            }
        }
    }
}
