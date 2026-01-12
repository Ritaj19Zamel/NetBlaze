using NetBlaze.SharedKernel.Dtos.Role.Responses;
using NetBlaze.SharedKernel.HelperUtilities.Constants;
using NetBlaze.SharedKernel.HelperUtilities.General;
using NetBlaze.Ui.Client.Services.CommonServices;

namespace NetBlaze.Ui.Client.Services
{
    public class BlazeRoleService : BaseBlazService
    {
        public BlazeRoleService(ExternalHttpClientWrapper externalHttpClientWrapper,
            CentralizedSnackbarProvider centralizedSnackbarProvider) : base(externalHttpClientWrapper, centralizedSnackbarProvider) { }

        public async Task<ApiResponse<List<GetRoleResponseDto>>> GetAllRolesAsync(CancellationToken cancellationToken = default)
        {
            var apiResponse = await _externalHttpClientWrapper.GetFromJsonAsync<ApiResponse<List<GetRoleResponseDto>>>
                (ApiRelativePaths.ROLE_GET_ALL, cancellationToken);
            return apiResponse;
        }

    }
}
