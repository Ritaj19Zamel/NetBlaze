using Microsoft.AspNetCore.Components;
using MudBlazor;
using NetBlaze.SharedKernel.Dtos.Department.Requests;
using NetBlaze.SharedKernel.Dtos.Department.Responses;
using NetBlaze.Ui.Client.Services;

namespace NetBlaze.Ui.Client.Dialogs.Department
{
    public partial class UpdateDepartmentDialog
    {
        [Inject] BlazeDepartmentService BlazeDepartmentService { get; set; } = null!;

        [CascadingParameter] IMudDialogInstance MudDialog { get; set; } = null!;

        [Parameter] public GetDepartmentResponseDto GetDepartmentResponseDto { get; set; } = null!;

        private UpdateDepartmentRequestDto _updateDepartmentRequestDto = null!;

        protected override void OnInitialized()
        {
            _updateDepartmentRequestDto = new UpdateDepartmentRequestDto
            {
                Id = GetDepartmentResponseDto.Id,
                Name = GetDepartmentResponseDto.Name
            };
        }

        private void Cancel()
        {
            MudDialog.Cancel();
        }

        private async Task SubmitAsync()
        {
            var response = await BlazeDepartmentService
                .UpdateDepartmentAsync(_updateDepartmentRequestDto);

            if (response.Success)
            {
                MudDialog.Close(DialogResult.Ok(_updateDepartmentRequestDto));
            }
        }
    }
}
