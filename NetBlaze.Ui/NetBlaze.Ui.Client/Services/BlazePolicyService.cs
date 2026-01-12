using NetBlaze.SharedKernel.Dtos.Policy.Requests;
using NetBlaze.SharedKernel.Dtos.Policy.Responses;
using NetBlaze.SharedKernel.HelperUtilities.Constants;
using NetBlaze.SharedKernel.HelperUtilities.General;
using NetBlaze.Ui.Client.InternalHelperTypes.General;
using NetBlaze.Ui.Client.Services.CommonServices;

namespace NetBlaze.Ui.Client.Services
{
    public class BlazePolicyService : BaseBlazService
    {
        public BlazePolicyService(ExternalHttpClientWrapper externalHttpClientWrapper,
            CentralizedSnackbarProvider centralizedSnackbarProvider) : base(externalHttpClientWrapper, centralizedSnackbarProvider) { }

        public async Task<ApiResponse<object>> CreatePolicyAsync(
           CreatePolicyRequestDto createPolicyRequestDto,
           CancellationToken cancellationToken = default)
        {
            var apiResponse = await _externalHttpClientWrapper.PostAsJsonAsync<CreatePolicyRequestDto,
                    ApiResponse<object>>(
                        ApiRelativePaths.POLICY_CREATE,
                        createPolicyRequestDto,
                        cancellationToken);

            _centralizedSnackbarProvider.ShowApiResponseSnackbar(apiResponse);

            return apiResponse;
        }

        public async Task<PaginatedList<GetPolicyResponseDto>> GetPoliciesPagedAsync(
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default)
        {
            var response = await _externalHttpClientWrapper.GetFromJsonAsync<ApiResponse<PaginatedList<GetPolicyResponseDto>>>(
                        $"{ApiRelativePaths.POLICY_GET_ALL}?pageNumber={pageNumber}&pageSize={pageSize}",
                        cancellationToken);

            return response.Success
                ? response.Data
                : new PaginatedList<GetPolicyResponseDto>();
        }

        public async Task<ApiResponse<GetPolicyResponseDto>> GetPolicyAsync(
            long id,
            CancellationToken cancellationToken = default)
        {
            return await _externalHttpClientWrapper.GetFromJsonAsync<
                ApiResponse<GetPolicyResponseDto>>(
                    $"{ApiRelativePaths.POLICY_GET_BY_ID}?id={id}",
                    cancellationToken);
        }

        public async Task<ApiResponse<object>> UpdatePolicyAsync(
            UpdatePolicyRequestDto updatePolicyRequestDto,
            CancellationToken cancellationToken = default)
        {
            var apiResponse = await _externalHttpClientWrapper.PutAsJsonAsync<UpdatePolicyRequestDto,
                    ApiResponse<object>>(
                        ApiRelativePaths.POLICY_UPDATE,
                        updatePolicyRequestDto,
                        cancellationToken);

            _centralizedSnackbarProvider.ShowApiResponseSnackbar(apiResponse);

            return apiResponse;
        }

        public async Task<ApiResponse<object>> DeletePolicyAsync(
            long id,
            CancellationToken cancellationToken = default)
        {
            var apiResponse = await _externalHttpClientWrapper.DeleteFromJsonAsync<
                    ApiResponse<object>>(
                        $"{ApiRelativePaths.POLICY_DELETE}?id={id}",
                        cancellationToken);

            _centralizedSnackbarProvider.ShowApiResponseSnackbar(apiResponse);

            return apiResponse;
        }
    }
}
