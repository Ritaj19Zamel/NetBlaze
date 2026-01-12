using Microsoft.AspNetCore.Components;
using MudBlazor;
using NetBlaze.SharedKernel.Dtos.RandomCheck.Requests;
using NetBlaze.SharedKernel.Dtos.User.Responses;
using NetBlaze.Ui.Client.Services;

namespace NetBlaze.Ui.Client.Pages
{
    public partial class GenerateOTPPage
    {
        [Inject] BlazeRandomCheckService RandomCheckService { get; set; } = null!;
        [Inject] BlazeUserService BlazeUserService { get; set; } = null!;
        [Inject] ISnackbar Snackbar { get; set; } = null!;

        private bool _SendToAllEmployees = false;
        private bool _isLoading;

        private List<GetEmployeeResponseDto> _employees = new();
        private IList<long> SelectedUsers { get; set; } = new List<long>();

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                var response = await BlazeUserService.GetEmployeesAsync();
                if (response.Success && response.Data != null)
                {
                    _employees = response.Data;
                    StateHasChanged();
                }
            }
        }

        private void OnSelectedUsersChanged(IEnumerable<long> values)
        {
            SelectedUsers = values.ToList();
        }

        private async Task SendOtpAsync()
        {
            _isLoading = true;

            var dto = new GenerateOtpRequestDto
            {
                SendToAllEmployees = _SendToAllEmployees,
                UserIds = _SendToAllEmployees ? [] : SelectedUsers.ToList()
            };

            var response = await RandomCheckService.GenerateOtpAsync(dto);

            _isLoading = false;

            Snackbar.Add(
                response.Message,
                response.Success ? Severity.Success : Severity.Error);
        }
        private string GetEmployeeName(long employeeId)
        {
            var employee = _employees.FirstOrDefault(e => e.Id == employeeId);
            return employee?.DisplayName ?? employeeId.ToString();
        }
    }
}
