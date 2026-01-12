using Microsoft.AspNetCore.Components;
using MudBlazor;
using NetBlaze.SharedKernel.Dtos.Vacation.Requests;
using NetBlaze.Ui.Client.Services;

namespace NetBlaze.Ui.Client.Dialogs.Vacation
{
    public partial class AddVacationDialog
    {
        [Inject] BlazeVacationService BlazeVacationService { get; set; } = null!;

        [CascadingParameter] IMudDialogInstance MudDialog { get; set; } = null!;

        private CreateVacationRequestDto _createVacationRequestDto = new();

        private DateTime? _dayDateProxy
        {
            get => _createVacationRequestDto.DayDate?.ToDateTime(TimeOnly.MinValue);
            set => _createVacationRequestDto.DayDate =
                value.HasValue ? DateOnly.FromDateTime(value.Value) : null;
        }

        private void Cancel()
        {
            MudDialog.Cancel();
        }

        private async Task SubmitAsync()
        {
            var response = await BlazeVacationService.CreateVacationAsync(_createVacationRequestDto);

            if (response.Success)
            {
                MudDialog.Close(DialogResult.Ok(_createVacationRequestDto));
            }
        }
    }
}
