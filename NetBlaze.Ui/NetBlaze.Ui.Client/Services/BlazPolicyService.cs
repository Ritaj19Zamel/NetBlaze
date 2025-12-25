using NetBlaze.SharedKernel.Dtos.Policy.Requests;
using NetBlaze.SharedKernel.Dtos.Policy.Responses;
using NetBlaze.SharedKernel.HelperUtilities.Constants;
using NetBlaze.SharedKernel.HelperUtilities.General;
using NetBlaze.Ui.Client.Services.CommonServices;

namespace NetBlaze.Ui.Client.Services
{
    public class BlazPolicyService : BaseBlazService
    {
        public BlazPolicyService(
            ExternalHttpClientWrapper externalHttpClientWrapper,
            CentralizedSnackbarProvider centralizedSnackbarProvider)
            : base(externalHttpClientWrapper, centralizedSnackbarProvider) { }

        public async Task<ApiResponse<object>> CreateAsync(
            CreatePolicyRequestDto createPolicyRequestDto,
            CancellationToken cancellationToken = default)
        {
            var apiResponse =
                await _externalHttpClientWrapper
                    .PostAsJsonAsync<CreatePolicyRequestDto, ApiResponse<object>>(
                        ApiRelativePaths.POLICY_CREATE,
                        createPolicyRequestDto,
                        cancellationToken);

            _centralizedSnackbarProvider.ShowApiResponseSnackbar(apiResponse);

            return apiResponse;
        }

        public async Task<ApiResponse<List<GetPolicyResponseDto>>> GetAllAsync(
            CancellationToken cancellationToken = default)
        {
            var apiResponse =
                await _externalHttpClientWrapper
                    .GetFromJsonAsync<ApiResponse<List<GetPolicyResponseDto>>>(
                        ApiRelativePaths.POLICY_GET_ALL,
                        cancellationToken);

            return apiResponse;
        }

        public async Task<ApiResponse<GetPolicyResponseDto>> GetByIdAsync(
            long id,
            CancellationToken cancellationToken = default)
        {
            var apiResponse =
                await _externalHttpClientWrapper
                    .GetFromJsonAsync<ApiResponse<GetPolicyResponseDto>>(
                        $"{ApiRelativePaths.POLICY_GET_BY_ID}?{nameof(id)}={id}",
                        cancellationToken);

            return apiResponse;
        }

        public async Task<ApiResponse<object>> UpdateAsync(
            long id,
            UpdatePolicyRequestDto updatePolicyRequestDto,
            CancellationToken cancellationToken = default)
        {
            var apiResponse =
                await _externalHttpClientWrapper
                    .PutAsJsonAsync<UpdatePolicyRequestDto, ApiResponse<object>>(
                        $"{ApiRelativePaths.POLICY_UPDATE}?{nameof(id)}={id}",
                        updatePolicyRequestDto,
                        cancellationToken);

            _centralizedSnackbarProvider.ShowApiResponseSnackbar(apiResponse);

            return apiResponse;
        }

        public async Task<ApiResponse<object>> DeleteAsync(
            long id,
            CancellationToken cancellationToken = default)
        {
            var apiResponse =
                await _externalHttpClientWrapper
                    .DeleteFromJsonAsync<ApiResponse<object>>(
                        $"{ApiRelativePaths.POLICY_DELETE}?{nameof(id)}={id}",
                        cancellationToken);

            _centralizedSnackbarProvider.ShowApiResponseSnackbar(apiResponse);

            return apiResponse;
        }
    }
}
