using Microsoft.AspNetCore.Components;
using MudBlazor;
using NetBlaze.SharedKernel.Dtos.User.Responses;
using NetBlaze.SharedKernel.SharedResources;
using NetBlaze.Ui.Client.Components.Generic;
using NetBlaze.Ui.Client.Dialogs.User;
using NetBlaze.Ui.Client.Services;

namespace NetBlaze.Ui.Client.Components.Concrete.User
{
    public partial class UserCrudOperationsComponent
    {
        [Inject] BlazeUserService BlazeUserService { get; set; } = null!;
        [Inject] IDialogService DialogService { get; set; } = null!;

        private MudTable<GetUserResponseDto>? _table;
        private bool _isLoading;

        private async Task<TableData<GetUserResponseDto>> LoadServerData(
            TableState state, CancellationToken cancellationToken)
        {
            _isLoading = true;

            var response = await BlazeUserService.GetAllUsersAsync(
                state.Page + 1,
                state.PageSize,
                cancellationToken);

            _isLoading = false;

            return new TableData<GetUserResponseDto>
            {
                Items = response.Data?.Items ?? [],
                TotalItems = response.Data?.TotalCount ?? 0
            };
        }
        private async Task OpenAddDialogAsync()
        {
            var dialog = await DialogService.ShowAsync<AddUserDialog>(
                Messages.AddUser,
                new DialogOptions { MaxWidth = MaxWidth.Large, FullWidth = true });

            if (!(await dialog.Result).Canceled)
            {
                await _table!.ReloadServerData();
            }
                
        }
        private async Task OpenUpdateDialogAsync(GetUserResponseDto user)
        {
            var parameters = new DialogParameters
            {
                { nameof(UpdateUserDialog.GetUserResponseDto), user }
            };

            var dialog = await DialogService.ShowAsync<UpdateUserDialog>(
                Messages.UpdateUser, parameters);

            if (!(await dialog.Result).Canceled)
                await _table!.ReloadServerData();
        }

        private async Task DeleteUserAsync(long id)
        {
            var parameters = new DialogParameters<GenericDialog>
            {
                { x => x.Title, Messages.DeleteUser},
                { x => x.Content, Messages.ConfirmDelete },
                { x => x.CancelText, Messages.Cancel },
                { x => x.SubmitText, Messages.DeleteUser }
            };

            var dialog = await DialogService.ShowAsync<GenericDialog>(string.Empty, parameters);


            if (!(await dialog.Result).Canceled)
            {
                await BlazeUserService.DeleteUserAsync(id);
                await _table!.ReloadServerData();
            }
        }
    }
}
