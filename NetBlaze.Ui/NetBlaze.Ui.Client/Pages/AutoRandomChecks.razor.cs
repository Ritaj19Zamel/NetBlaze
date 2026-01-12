using Microsoft.AspNetCore.Components;
using MudBlazor;
using NetBlaze.SharedKernel.Dtos.RandomCheck.Requests;
using NetBlaze.Ui.Client.Services;

namespace NetBlaze.Ui.Client.Pages
{
    public partial class AutoRandomChecks
    {
        [Inject] BlazeRandomCheckService RandomCheckService { get; set; } = null!;
        [Inject] ISnackbar Snackbar { get; set; } = null!;

        private bool _isLoading;

        private DateTime? _fromDate = DateTime.Today;
        private DateTime? _toDate = DateTime.Today.AddDays(7);
        private TimeSpan? _fromTime;
        private TimeSpan? _toTime;

        private AutoRandomCheckRequestDto Model = new()
        {
            IsEnabled = true,
            ChecksPerDay = 1,
            FromTime = new TimeSpan(9, 0, 0),
            ToTime = new TimeSpan(17, 0, 0)
        };

        protected override void OnInitialized()
        {
            _fromTime = Model.FromTime;
            _toTime = Model.ToTime;
        }

        private async Task SaveConfigAsync()
        {
            _isLoading = true;

            Model.FromDate = DateOnly.FromDateTime(_fromDate.Value);
            Model.ToDate = DateOnly.FromDateTime(_toDate.Value);
            Model.FromTime = _fromTime.Value;
            Model.ToTime = _toTime.Value;

            var response =
                await RandomCheckService.SaveAutoRandomCheckConfigAsync(Model);

            _isLoading = false;

            Snackbar.Add(
                response.Message,
                response.Success ? Severity.Success : Severity.Error);
        }
    }
}
