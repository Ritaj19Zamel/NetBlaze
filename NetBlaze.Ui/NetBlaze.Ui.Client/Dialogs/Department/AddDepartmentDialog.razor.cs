using Microsoft.AspNetCore.Components;
using MudBlazor;
using NetBlaze.SharedKernel.Dtos.Department.Requests;
using NetBlaze.Ui.Client.Services;

namespace NetBlaze.Ui.Client.Dialogs.Department
{
    public partial class AddDepartmentDialog
    {
        [Inject] BlazeDepartmentService BlazeDepartmentService { get; set; } = null!;

        [CascadingParameter] IMudDialogInstance MudDialog { get; set; } = null!;

        private CreateDepartmentRequestDto _addDepartmentRequestDto = new();

        private void Cancel()
        {
            MudDialog.Cancel();
        }
        private async Task SubmitAsync()
        {
            var response = await BlazeDepartmentService
                .CreateDepartmentAsync(_addDepartmentRequestDto);

            if (response.Success)
            {
                MudDialog.Close(DialogResult.Ok(_addDepartmentRequestDto));
            }
        }
    }
}
