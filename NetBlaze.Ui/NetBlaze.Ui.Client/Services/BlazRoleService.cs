using NetBlaze.SharedKernel.Dtos.Role.Responses;
using NetBlaze.SharedKernel.HelperUtilities.Constants;
using NetBlaze.SharedKernel.HelperUtilities.General;
using NetBlaze.Ui.Client.Services.CommonServices;

namespace NetBlaze.Ui.Client.Services
{
    public class BlazRoleService : BaseBlazService
    {
        public BlazRoleService(
            ExternalHttpClientWrapper externalHttpClientWrapper,
            CentralizedSnackbarProvider centralizedSnackbarProvider)
            : base(externalHttpClientWrapper, centralizedSnackbarProvider) { }

        public async Task<ApiResponse<List<GetRoleResponseDto>>> GetAllAsync(
            CancellationToken cancellationToken = default)
        {
            var apiResponse =
                await _externalHttpClientWrapper
                    .GetFromJsonAsync<ApiResponse<List<GetRoleResponseDto>>>(
                        ApiRelativePaths.ROLE_GET_ALL,
                        cancellationToken);

            return apiResponse;
        }

        public async Task<ApiResponse<GetRoleResponseDto>> GetByIdAsync(
            long id,
            CancellationToken cancellationToken = default)
        {
            var apiResponse =
                await _externalHttpClientWrapper
                    .GetFromJsonAsync<ApiResponse<GetRoleResponseDto>>(
                        $"{ApiRelativePaths.ROLE_GET_BY_ID}/{id}",
                        cancellationToken);

            return apiResponse;
        }

    }
}
