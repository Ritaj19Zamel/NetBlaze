using Microsoft.AspNetCore.Components;
using MudBlazor;
using NetBlaze.SharedKernel.Dtos.Vacation.Responses;
using NetBlaze.SharedKernel.SharedResources;
using NetBlaze.Ui.Client.Components.Generic;
using NetBlaze.Ui.Client.Dialogs.Vacation;
using NetBlaze.Ui.Client.Services;

namespace NetBlaze.Ui.Client.Components.Concrete.Vacation
{
    public partial class VacationCrudOperationsComponent
    {
        [Inject] IDialogService DialogService { get; set; } = null!;
        [Inject] BlazeVacationService BlazeVacationService { get; set; } = null!;

        private MudTable<GetVacationResponseDto>? _table;
        private bool _isLoading;

        private async Task<TableData<GetVacationResponseDto>> LoadServerData(
            TableState state,
            CancellationToken cancellationToken)
        {
            _isLoading = true;

            var pageNumber = state.Page + 1; 
            var pageSize = state.PageSize;

            var response = await BlazeVacationService
                .GetVacationsPagedAsync(pageNumber, pageSize, cancellationToken);

            _isLoading = false;

            return new TableData<GetVacationResponseDto>
            {
                Items = response.Items,
                TotalItems = response.TotalCount
            };
        }

        private async Task OpenAddDialogAsync()
        {
            var dialog = await DialogService.ShowAsync<AddVacationDialog>(
                Messages.AddVacation,
                new DialogOptions { MaxWidth = MaxWidth.Large, FullWidth = true });

            if (!(await dialog.Result).Canceled)
                await _table!.ReloadServerData();
        }

        private async Task OpenUpdateDialogAsync(GetVacationResponseDto vacation)
        {
            var response = await BlazeVacationService.GetVacationAsync(vacation.Id);
            if (!response.Success) return;

            var parameters = new DialogParameters
            {
                { nameof(UpdateVacationDialog.GetVacationResponseDto), response.Data }
            };

            var dialog = await DialogService.ShowAsync<UpdateVacationDialog>(
                Messages.UpdateVacation, parameters);

            if (!(await dialog.Result).Canceled)
                await _table!.ReloadServerData();
        }

        private async Task DeleteVacationAsync(long id)
        {
            var parameters = new DialogParameters<GenericDialog>
            {
                { x => x.Title, Messages.DeleteVacation },
                { x => x.Content, Messages.ConfirmDelete },
                { x => x.CancelText, Messages.Cancel },
                { x => x.SubmitText, Messages.DeleteVacation }
            };

            var dialog = await DialogService.ShowAsync<GenericDialog>(string.Empty, parameters);

            if (!(await dialog.Result).Canceled)
            {
                await BlazeVacationService.DeleteVacationAsync(id);
                await _table!.ReloadServerData();
            }
        }
    }
}
