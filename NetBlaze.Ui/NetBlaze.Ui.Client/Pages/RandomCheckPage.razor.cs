using Microsoft.AspNetCore.Components;
using MudBlazor;
using NetBlaze.SharedKernel.Dtos.RandomCheck.Requests;
using NetBlaze.SharedKernel.Dtos.RandomCheck.Responses;
using NetBlaze.SharedKernel.Dtos.User.Responses;
using NetBlaze.Ui.Client.Services;

namespace NetBlaze.Ui.Client.Pages
{
    public partial class RandomCheckPage
    {
        [Inject] BlazeRandomCheckService RandomCheckService { get; set; } = default!;
        [Inject] BlazeUserService UserService { get; set; } = default!;
        [Inject] ISnackbar Snackbar { get; set; } = default!;

        private MudTable<GetUserRandomChecksResponseDto>? _table;
        private List<GetEmployeeResponseDto> _employees = [];

        private DateTime? _fromDate = DateTime.Today.AddDays(-7);
        private DateTime? _toDate = DateTime.Today;

        private GetUserRandomChecksRequestDto _request = new()
        {
            Pagination = { PageNumber = 1, PageSize = 10 }
        };

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                var employeesResponse = await UserService.GetEmployeesAsync();
                if (employeesResponse.Success)
                {
                    _employees = employeesResponse.Data!;
                    StateHasChanged();
                }
            }
        }

        private string GetEmployeeName(long id) => _employees.FirstOrDefault(e => e.Id == id)?.DisplayName ?? id.ToString();

        private async Task<TableData<GetUserRandomChecksResponseDto>> LoadChecksAsync(
            TableState state,
            CancellationToken cancellationToken)
        {
            _request.Pagination.PageNumber = state.Page + 1;
            _request.Pagination.PageSize = state.PageSize;
            _request.From = _fromDate!.Value;
            _request.To = _toDate!.Value;

            var response = await RandomCheckService
                .GetUserChecksAsync(_request, cancellationToken);

            if (!response.Success)
            {
                Snackbar.Add(response.Message, Severity.Warning);
                return new TableData<GetUserRandomChecksResponseDto>();
            }

            return new TableData<GetUserRandomChecksResponseDto>
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
