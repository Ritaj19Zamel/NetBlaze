using Microsoft.AspNetCore.Components;
using MudBlazor;
using NetBlaze.SharedKernel.Dtos.Attendence.Requests;
using NetBlaze.SharedKernel.Dtos.Attendence.Responses;
using NetBlaze.Ui.Client.Services;

namespace NetBlaze.Ui.Client.Pages
{
    public partial class AttendanceReportPage
    {
        [Inject] BlazeAttendenceService BlazeAttendenceService { get; set; } = default!;
        [Inject] ISnackbar Snackbar { get; set; } = default!;

        private MudTable<GetAttendanceResponseDto>? _table;
        private bool _isLoading;

        private DateTime? _fromDate = DateTime.Today.AddDays(-7);
        private DateTime? _toDate = DateTime.Today;


        private GetAttendanceRequestDto _request = new()
        {
            From = DateOnly.FromDateTime(DateTime.Today.AddDays(-7)),
            To = DateOnly.FromDateTime(DateTime.Today)
        };

        private async Task<TableData<GetAttendanceResponseDto>> LoadAttendanceAsync(
                        TableState state,
                        CancellationToken cancellationToken)
        {
            _isLoading = true;

            _request.Pagination.PageNumber = state.Page + 1;
            _request.Pagination.PageSize = state.PageSize;

            _request.From = DateOnly.FromDateTime(_fromDate!.Value);
            _request.To = DateOnly.FromDateTime(_toDate!.Value);

            var response = await BlazeAttendenceService
                .GetAttendanceReportAsync(_request, cancellationToken);

            _isLoading = false;

            if (!response.Success)
            {
                Snackbar.Add(response.Message, Severity.Warning);
                return new TableData<GetAttendanceResponseDto>
                {
                    Items = Array.Empty<GetAttendanceResponseDto>(),
                    TotalItems = 0
                };
            }

            return new TableData<GetAttendanceResponseDto>
            {
                Items = response.Data!.Items,
                TotalItems = response.Data.TotalCount
            };
        }


        private void ReloadTable()
        {
            _table?.ReloadServerData();
        }
    }
}
