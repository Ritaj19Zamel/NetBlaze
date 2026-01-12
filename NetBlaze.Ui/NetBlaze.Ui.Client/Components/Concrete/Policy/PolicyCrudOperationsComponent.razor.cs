using Microsoft.AspNetCore.Components;
using MudBlazor;
using NetBlaze.SharedKernel.Dtos.Policy.Responses;
using NetBlaze.SharedKernel.SharedResources;
using NetBlaze.Ui.Client.Components.Generic;
using NetBlaze.Ui.Client.Dialogs.Policy;
using NetBlaze.Ui.Client.Services;

namespace NetBlaze.Ui.Client.Components.Concrete.Policy
{
    public partial class PolicyCrudOperationsComponent
    {
        [Inject] IDialogService DialogService { get; set; } = null!;

        [Inject] BlazePolicyService BlazePolicyService { get; set; } = null!;
        [Inject] ISnackbar Snackbar { get; set; } = null!;


        private MudTable<GetPolicyResponseDto>? _table;
        private bool _isLoading;

        private async Task<TableData<GetPolicyResponseDto>> LoadServerData(
            TableState state,
            CancellationToken cancellationToken)
        {
            _isLoading = true;

            var pageNumber = state.Page + 1;
            var pageSize = state.PageSize;

            var response = await BlazePolicyService
                .GetPoliciesPagedAsync(pageNumber, pageSize, cancellationToken);

            _isLoading = false;

            return new TableData<GetPolicyResponseDto>
            {
                Items = response.Items,
                TotalItems = response.TotalCount
            };
        }

        private async Task OpenAddDialogAsync()
        {
            var dialog = await DialogService.ShowAsync<AddPolicyDialog>(
                Messages.AddPolicy,
                new DialogOptions { MaxWidth = MaxWidth.Large, FullWidth = true });

            if (!(await dialog.Result).Canceled)
                await _table!.ReloadServerData();
        }

        private async Task OpenUpdateDialogAsync(long policyId)
        {
            var response = await BlazePolicyService.GetPolicyAsync(policyId);
            if (!response.Success) return;

            var parameters = new DialogParameters
            {
                { nameof(UpdatePolicyDialog.GetPolicyResponseDto), response.Data }
            };

            var dialog = await DialogService.ShowAsync<UpdatePolicyDialog>(
                Messages.UpdatePolicy,
                parameters,
                new DialogOptions { MaxWidth = MaxWidth.Large, FullWidth = true });

            if (!(await dialog.Result).Canceled)
                await _table!.ReloadServerData();
        }

        private async Task DeletePolicyAsync(long id)
        {
            var parameters = new DialogParameters<GenericDialog>
            {
                { x => x.Title, Messages.DeletePolicy },
                { x => x.Content, Messages.ConfirmDelete },
                { x => x.CancelText, Messages.Cancel },
                { x => x.SubmitText, Messages.DeletePolicy }
            };

            var dialog = await DialogService.ShowAsync<GenericDialog>(
                string.Empty,
                parameters,
                new DialogOptions { MaxWidth = MaxWidth.Small });

            if (!(await dialog.Result).Canceled)
            {
                await BlazePolicyService.DeletePolicyAsync(id);
                await _table!.ReloadServerData();
            }
        }
    }
}
