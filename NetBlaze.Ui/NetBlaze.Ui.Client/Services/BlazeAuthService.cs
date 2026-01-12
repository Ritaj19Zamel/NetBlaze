using NetBlaze.SharedKernel.Dtos.Auth.Requests;
using NetBlaze.SharedKernel.Dtos.Auth.Responses;
using NetBlaze.SharedKernel.HelperUtilities.Constants;
using NetBlaze.SharedKernel.HelperUtilities.General;
using NetBlaze.Ui.Client.Services.CommonServices;

namespace NetBlaze.Ui.Client.Services
{
    public class BlazeAuthService : BaseBlazService
    {
        public BlazeAuthService(ExternalHttpClientWrapper externalHttpClientWrapper,
            CentralizedSnackbarProvider centralizedSnackbarProvider) : base(externalHttpClientWrapper, centralizedSnackbarProvider) { }

        public async Task<ApiResponse<RegisterResponseDto>> RegisterAsync(RegisterRequestDto registerRequestDto, CancellationToken cancellationToken = default)
        {
            var apiResponse = await _externalHttpClientWrapper.PostAsJsonAsync<RegisterRequestDto, ApiResponse<RegisterResponseDto>>
                (ApiRelativePaths.AUTH_REGISTER, registerRequestDto, cancellationToken);

            _centralizedSnackbarProvider.ShowApiResponseSnackbar(apiResponse);

            return apiResponse;
        }

        public async Task<ApiResponse<LoginResponseDto>> LoginAsync(LoginRequestDto loginRequestDto, CancellationToken cancellationToken = default)
        {
            var apiResponse = await _externalHttpClientWrapper.PostAsJsonAsync<LoginRequestDto, ApiResponse<LoginResponseDto>>
                (ApiRelativePaths.AUTH_LOGIN, loginRequestDto, cancellationToken);

            _centralizedSnackbarProvider.ShowApiResponseSnackbar(apiResponse);

            return apiResponse;
        }
        public async Task<ApiResponse<object>> ForgetPasswordAsync(ForgetPasswordRequestDto dto, CancellationToken cancellationToken = default)
        {
            return await _externalHttpClientWrapper
                .PostAsJsonAsync<ForgetPasswordRequestDto, ApiResponse<object>>(
                    ApiRelativePaths.AUTH_FORGET_PASSWORD,
                    dto,
                    cancellationToken);
        }

        public async Task<ApiResponse<object>> ResetPasswordAsync(ResetPasswordRequestDto dto, CancellationToken cancellationToken = default)
        {
            return await _externalHttpClientWrapper
                .PostAsJsonAsync<ResetPasswordRequestDto, ApiResponse<object>>(
                    ApiRelativePaths.AUTH_RESET_PASSWORD,
                    dto,
                    cancellationToken);
        }



    }
}
