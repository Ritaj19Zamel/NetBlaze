using Microsoft.AspNetCore.Components;
using MudBlazor;
using NetBlaze.SharedKernel.Dtos.Attendence.Requests;
using Microsoft.Extensions.Localization;
using NetBlaze.SharedKernel.SharedResources;
using NetBlaze.SharedKernel.Enums;
using NetBlaze.Ui.Client.Services;

namespace NetBlaze.Ui.Client.Pages
{
    public partial class TodayAttendancePage
    {
        [Inject] BlazeAttendenceService BlazeAttendenceService { get; set; } = default!;

        [Inject] IStringLocalizer<Messages> Localizer { get; set; } = default!;

        [Inject] ISnackbar Snackbar { get; set; } = default!;

        private bool _isLoading = true;
        private bool _isProcessing;

        private TodayAttendanceStatus _status;

        private string _lastCheckInText = "—";
        private string _lastCheckOutText = "—";

        private bool _isClientReady;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await LoadTodayAttendance();
                StateHasChanged();
            }
        }


        private async Task LoadTodayAttendance()
        {
            _isLoading = true;

            var response = await BlazeAttendenceService.GetTodayAttendanceAsync();

            if (response.Success)
            {
                var data = response.Data;

                if (data.CheckIn == null)
                {
                    _status = TodayAttendanceStatus.NotCheckedIn;
                }
                else if (data.CheckOut == null)
                {
                    _status = TodayAttendanceStatus.CheckedIn;
                    _lastCheckInText = data.CheckIn.Value.ToString("hh:mm tt");
                }
                else
                {
                    _status = TodayAttendanceStatus.Completed;
                    _lastCheckInText = data.CheckIn.Value.ToString("hh:mm tt");
                    _lastCheckOutText = data.CheckOut.Value.ToString("hh:mm tt");
                }
            }

            _isLoading = false;
        }

        private async Task RecordAttendance()
        {
            _isProcessing = true;

            var response = await BlazeAttendenceService.AddAttendanceAsync();

            _isProcessing = false;

            if (response.Success)
            {
                Snackbar.Add(response.Message, Severity.Success);
                await LoadTodayAttendance();
            }
            else
            {
                Snackbar.Add(response.Message, Severity.Error);
            }
        }

        private Color StatusColor =>
            _status switch
            {
                TodayAttendanceStatus.NotCheckedIn => Color.Default,
                TodayAttendanceStatus.CheckedIn => Color.Warning,
                TodayAttendanceStatus.Completed => Color.Success,
                _ => Color.Default
            };

        
    }
}
