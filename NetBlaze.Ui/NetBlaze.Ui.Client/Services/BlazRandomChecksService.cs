using NetBlaze.SharedKernel.Dtos.OTP.Requests;
using NetBlaze.SharedKernel.HelperUtilities.Constants;
using NetBlaze.SharedKernel.HelperUtilities.General;
using NetBlaze.Ui.Client.Services.CommonServices;

namespace NetBlaze.Ui.Client.Services
{
    public class BlazRandomChecksService : BaseBlazService
    {
        public BlazRandomChecksService(
            ExternalHttpClientWrapper externalHttpClientWrapper,
            CentralizedSnackbarProvider centralizedSnackbarProvider)
            : base(externalHttpClientWrapper, centralizedSnackbarProvider) { }

        public async Task<ApiResponse<object>> GenerateOTPAsync(
            GenerateOtpRequestDto generateOtpRequestDto,
            CancellationToken cancellationToken = default)
        {
            var apiResponse =
                await _externalHttpClientWrapper
                    .PostAsJsonAsync<GenerateOtpRequestDto, ApiResponse<object>>(
                        ApiRelativePaths.RANDOM_CHECKS_GENERATE_OTP,
                        generateOtpRequestDto,
                        cancellationToken);

            _centralizedSnackbarProvider.ShowApiResponseSnackbar(apiResponse);

            return apiResponse;
        }

        public async Task<ApiResponse<object>> VerifyOTPAsync(
            VerifyOTPRequestDto verifyOTPRequestDto,
            CancellationToken cancellationToken = default)
        {
            var apiResponse =
                await _externalHttpClientWrapper
                    .PostAsJsonAsync<VerifyOTPRequestDto, ApiResponse<object>>(
                        ApiRelativePaths.RANDOM_CHECKS_VERIFY_OTP,
                        verifyOTPRequestDto,
                        cancellationToken);

            _centralizedSnackbarProvider.ShowApiResponseSnackbar(apiResponse);

            return apiResponse;
        }
    }
}
