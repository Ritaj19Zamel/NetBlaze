using NetBlaze.SharedKernel.Dtos.Attendence.Requests;
using NetBlaze.SharedKernel.Dtos.Attendence.Responses;
using NetBlaze.SharedKernel.HelperUtilities.Constants;
using NetBlaze.SharedKernel.HelperUtilities.General;
using NetBlaze.Ui.Client.InternalHelperTypes.General;
using NetBlaze.Ui.Client.Services.CommonServices;

namespace NetBlaze.Ui.Client.Services
{
    public class BlazeAttendenceService : BaseBlazService
    {
        public BlazeAttendenceService(ExternalHttpClientWrapper externalHttpClientWrapper,
            CentralizedSnackbarProvider centralizedSnackbarProvider) : base(externalHttpClientWrapper, centralizedSnackbarProvider) { }

        public async Task<ApiResponse<object>> AddAttendanceAsync(CancellationToken cancellationToken = default)
        {
            var apiResponse =
                await _externalHttpClientWrapper.PostAsJsonAsync<object, ApiResponse<object>>(
                    ApiRelativePaths.ATTENDANCE_ADD,
                    new { },
                    cancellationToken);

            _centralizedSnackbarProvider.ShowApiResponseSnackbar(apiResponse);
            return apiResponse;
        }

        public async Task<ApiResponse<object>> ApprovePolicyRequestAsync(ApprovePolicyRequestDto approvePolicyRequestDto,
            CancellationToken cancellationToken = default)
        {
            var apiResponse =
                await _externalHttpClientWrapper.PostAsJsonAsync<
                    ApprovePolicyRequestDto,
                    ApiResponse<object>>(
                    ApiRelativePaths.ATTENDANCE_APPROVE_POLICY,
                    approvePolicyRequestDto,
                    cancellationToken);

            _centralizedSnackbarProvider.ShowApiResponseSnackbar(apiResponse);
            return apiResponse;
        }

        public async Task<ApiResponse<PaginatedList<GetAttendanceResponseDto>>>GetAttendanceReportAsync(
                GetAttendanceRequestDto getAttendanceRequestDto,CancellationToken cancellationToken = default)
        {
            var url =
                $"{ApiRelativePaths.ATTENDANCE_GET_REPORT}" +
                $"?Pagination.PageNumber={getAttendanceRequestDto.Pagination.PageNumber}" +
                $"&Pagination.PageSize={getAttendanceRequestDto.Pagination.PageSize}" +
                $"&From={getAttendanceRequestDto.From:yyyy-MM-dd}" +
                $"&To={getAttendanceRequestDto.To:yyyy-MM-dd}";

            return await _externalHttpClientWrapper
                .GetFromJsonAsync<ApiResponse<PaginatedList<GetAttendanceResponseDto>>>(
                    url,
                    cancellationToken);
        }

        public async Task<ApiResponse<PaginatedList<GetCheckInViolationResponseDto>>>
            GetCheckInViolationsAsync(GetCheckInViolationsRequestDto getCheckInViolationsRequestDto,
                CancellationToken cancellationToken = default)
        {
            var url =
                $"{ApiRelativePaths.ATTENDANCE_GET_CHECK_VIOLATIONS}" +
               $"?Pagination.PageNumber={getCheckInViolationsRequestDto.Pagination.PageNumber}" +
               $"&Pagination.PageSize={getCheckInViolationsRequestDto.Pagination.PageSize}"+
                $"&FromDate={getCheckInViolationsRequestDto.FromDate:yyyy-MM-dd}" +
                $"&ToDate={getCheckInViolationsRequestDto.ToDate:yyyy-MM-dd}"
                + (getCheckInViolationsRequestDto.ViolationStatus.HasValue? 
                $"&ViolationStatus={getCheckInViolationsRequestDto.ViolationStatus.Value}": ""); ;

            return await _externalHttpClientWrapper
                .GetFromJsonAsync<ApiResponse<PaginatedList<GetCheckInViolationResponseDto>>>(
                    url,
                    cancellationToken);
        }

        public async Task<ApiResponse<GetTodayAttendanceResponseDto>>GetTodayAttendanceAsync(CancellationToken cancellationToken = default)
        {
            return await _externalHttpClientWrapper
                .GetFromJsonAsync<ApiResponse<GetTodayAttendanceResponseDto>>(
                    ApiRelativePaths.ATTENDANCE_GET_TODAY_ATTENDANCE,
                    cancellationToken);
        }

    }
}
