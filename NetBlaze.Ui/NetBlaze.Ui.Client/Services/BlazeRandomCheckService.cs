using NetBlaze.SharedKernel.Dtos.RandomCheck.Requests;
using NetBlaze.SharedKernel.Dtos.RandomCheck.Responses;
using NetBlaze.SharedKernel.HelperUtilities.Constants;
using NetBlaze.SharedKernel.HelperUtilities.General;
using NetBlaze.Ui.Client.InternalHelperTypes.General;
using NetBlaze.Ui.Client.Services.CommonServices;

namespace NetBlaze.Ui.Client.Services
{
    public class BlazeRandomCheckService : BaseBlazService
    {
        public BlazeRandomCheckService(
            ExternalHttpClientWrapper http,
            CentralizedSnackbarProvider snackbar)
            : base(http, snackbar) { }

        public async Task<ApiResponse<object>> GenerateOtpAsync(
            GenerateOtpRequestDto dto,
            CancellationToken cancellationToken = default)
        {
            return await _externalHttpClientWrapper
                .PostAsJsonAsync<GenerateOtpRequestDto, ApiResponse<object>>(
                    ApiRelativePaths.RANDOMCHECK_GENERATE_OTP,
                    dto,
                    cancellationToken);
        }

        public async Task<ApiResponse<PaginatedList<GetUserRandomChecksResponseDto>>> GetUserChecksAsync(
            GetUserRandomChecksRequestDto dto,
            CancellationToken cancellationToken = default)
        {
            var url =
                $"{ApiRelativePaths.RANDOMCHECK_GET_USER_CHECKS}" +
                $"?UserId={dto.UserId}" +
                $"&From={dto.From:yyyy-MM-dd}" +
                $"&To={dto.To:yyyy-MM-dd}" +
                $"&Pagination.PageNumber={dto.Pagination.PageNumber}" +
                $"&Pagination.PageSize={dto.Pagination.PageSize}";

            return await _externalHttpClientWrapper
                .GetFromJsonAsync<ApiResponse<PaginatedList<GetUserRandomChecksResponseDto>>>(
                    url,
                    cancellationToken);
        }

        public async Task<ApiResponse<object>> VerifyOtpAsync(VerifyOTPRequestDto dto,CancellationToken cancellationToken = default)
        {
            return await _externalHttpClientWrapper
                .PostAsJsonAsync<VerifyOTPRequestDto, ApiResponse<object>>(
                    ApiRelativePaths.RANDOMCHECK_VERIFY_OTP,
                    dto,
                    cancellationToken);
        }

        public async Task<ApiResponse<object>> SaveAutoRandomCheckConfigAsync(
            AutoRandomCheckRequestDto dto,
            CancellationToken cancellationToken = default)
        {
            return await _externalHttpClientWrapper
                .PostAsJsonAsync<AutoRandomCheckRequestDto, ApiResponse<object>>(
                    ApiRelativePaths.RANDOMCHECK_SAVE_AUTO_RANDOMCHECK_CONFIG,
                    dto,
                    cancellationToken);
        }

    }
}
