using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;
using NetBlaze.SharedKernel.Dtos.Attendence.Requests;
using NetBlaze.SharedKernel.Dtos.Attendence.Responses;
using NetBlaze.SharedKernel.Enums;
using NetBlaze.SharedKernel.SharedResources;
using NetBlaze.Ui.Client.Dialogs.Attendence;
using NetBlaze.Ui.Client.Services;

namespace NetBlaze.Ui.Client.Pages.Attendance
{
    public partial class CheckInViolationsPage
    {
        [Inject] BlazeAttendenceService BlazeAttendenceService { get; set; } = default!;
        [Inject] ISnackbar Snackbar { get; set; } = default!;
        [Inject] IDialogService DialogService { get; set; } = default!;
        [Inject] IStringLocalizer<Messages> Localizer { get; set; } = default!;

        private MudTable<GetCheckInViolationResponseDto>? _table;
        private bool _isLoading;

        private DateTime? _fromDate = DateTime.Today.AddDays(-7);
        private DateTime? _toDate = DateTime.Today;
        private ViolationStatus? _status;


        private async Task<TableData<GetCheckInViolationResponseDto>> LoadViolationsAsync(
            TableState state,
            CancellationToken cancellationToken)
        {
            _isLoading = true;

            var request = new GetCheckInViolationsRequestDto
            {
                FromDate = DateOnly.FromDateTime(_fromDate!.Value),
                ToDate = DateOnly.FromDateTime(_toDate!.Value),
                ViolationStatus = _status,
                Pagination =
                {
                    PageNumber = state.Page + 1,
                    PageSize = state.PageSize
                }
            };


            var response = await BlazeAttendenceService
                .GetCheckInViolationsAsync(request, cancellationToken);

            _isLoading = false;

            if (!response.Success || response.Data == null)
            {
                Snackbar.Add(response.Message, Severity.Warning);
                return new TableData<GetCheckInViolationResponseDto>
                {
                    Items = [],
                    TotalItems = 0
                };
            }

            return new TableData<GetCheckInViolationResponseDto>
            {
                Items = response.Data.Items,
                TotalItems = response.Data.TotalCount
            };
        }

        private void ReloadTable()
        {
            _table?.ReloadServerData();
        }

        private async Task OpenDecisionDialog(GetCheckInViolationResponseDto row,  bool isApproved)
        {
            var parameters = new DialogParameters
            {
                ["IsApproved"] = isApproved
            };

            var dialog = DialogService.Show<ViolationDecisionDialog>(
                isApproved ? Messages.Approve : Messages.Reject,
                parameters);

            var result = await dialog.Result;

            if (result.Canceled)
                return;

            var clarification = result.Data?.ToString();

            var dto = new ApprovePolicyRequestDto
            {
                UserId = row.UserId,
                PolicyId = row.PolicyId,
                FromDate = DateOnly.FromDateTime(_fromDate!.Value),
                ToDate = DateOnly.FromDateTime(_toDate!.Value),
                IsApplied = isApproved,
                Clarification = clarification
            };

            var response =
                await BlazeAttendenceService.ApprovePolicyRequestAsync(dto);

            if (response.Success)
            {
                Snackbar.Add(response.Message, Severity.Success);
                ReloadTable();
            }
            else
            {
                Snackbar.Add(response.Message, Severity.Error);
            }
        }

    }
}
