using NetBlaze.SharedKernel.HelperUtilities.Constants;
using NetBlaze.SharedKernel.HelperUtilities.General;
using NetBlaze.Ui.Client.Services.CommonServices;

namespace NetBlaze.Ui.Client.Services
{
    public class BlazAttendanceService : BaseBlazService
    {
        public BlazAttendanceService(
            ExternalHttpClientWrapper externalHttpClientWrapper,
            CentralizedSnackbarProvider centralizedSnackbarProvider)
            : base(externalHttpClientWrapper, centralizedSnackbarProvider) { }

        public async Task<ApiResponse<string>> AddAttendanceAsync(
            CancellationToken cancellationToken = default)
        {
            var apiResponse =
                await _externalHttpClientWrapper
                    .PostAsJsonAsync<object, ApiResponse<string>>(
                        ApiRelativePaths.ATTENDANCE_ADD,
                        null,
                        cancellationToken);

            _centralizedSnackbarProvider.ShowApiResponseSnackbar(apiResponse);

            return apiResponse;
        }

        public async Task ProcessDailyAttendanceAsync(
            long userId,
            DateOnly date,
            CancellationToken cancellationToken = default)
        {
            await _externalHttpClientWrapper
                .PostAsJsonAsync<object, object>(
                    $"{ApiRelativePaths.ATTENDANCE_PROCESS_DAILY}" +
                    $"?{nameof(userId)}={userId}&{nameof(date)}={date}",
                    null,
                    cancellationToken);
        }
    }
}
