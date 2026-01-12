using Microsoft.AspNetCore.Components;
using MudBlazor;
using NetBlaze.SharedKernel.Dtos.Vacation.Requests;
using NetBlaze.SharedKernel.Dtos.Vacation.Responses;
using NetBlaze.Ui.Client.Services;

namespace NetBlaze.Ui.Client.Dialogs.Vacation
{
    public partial class UpdateVacationDialog
    {
        [Inject] BlazeVacationService BlazeVacationService { get; set; } = null!;

        [CascadingParameter] IMudDialogInstance MudDialog { get; set; } = null!;

        [Parameter] public GetVacationResponseDto GetVacationResponseDto { get; set; } = null!;

        private UpdateVacationRequestDto _updateVacationRequestDto = new();

        private DateTime? _dayDateProxy
        {
            get => _updateVacationRequestDto.DayDate?.ToDateTime(TimeOnly.MinValue);
            set => _updateVacationRequestDto.DayDate =
                value.HasValue ? DateOnly.FromDateTime(value.Value) : null;
        }

        protected override void OnInitialized()
        {
            if (GetVacationResponseDto != null)
            {
                _updateVacationRequestDto = new UpdateVacationRequestDto
                {
                    Id = GetVacationResponseDto.Id,
                    DayName = GetVacationResponseDto.DayName,
                    DayDate = GetVacationResponseDto.DayDate,
                    IsVacation = GetVacationResponseDto.IsVacation,
                    IsRecurring = GetVacationResponseDto.IsRecurring
                };
            }
        }

        private void Cancel()
        {
            MudDialog.Cancel();
        }

        private async Task SubmitAsync()
        {
            var response = await BlazeVacationService.UpdateVacationAsync(_updateVacationRequestDto);

            if (response.Success)
            {
                MudDialog.Close(DialogResult.Ok(_updateVacationRequestDto));
            }
        }
    }
}
