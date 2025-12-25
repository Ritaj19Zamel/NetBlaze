using NetBlaze.SharedKernel.Dtos.Department.Responses;
using NetBlaze.SharedKernel.HelperUtilities.Constants;
using NetBlaze.SharedKernel.HelperUtilities.General;
using NetBlaze.Ui.Client.Services.CommonServices;

namespace NetBlaze.Ui.Client.Services
{
    public class BlazDepartmentService : BaseBlazService
    {
        public BlazDepartmentService(
            ExternalHttpClientWrapper externalHttpClientWrapper,
            CentralizedSnackbarProvider centralizedSnackbarProvider)
            : base(externalHttpClientWrapper, centralizedSnackbarProvider) { }

        public async Task<ApiResponse<List<GetDepartmentResponseDto>>> GetAllAsync(
            CancellationToken cancellationToken = default)
        {
            var apiResponse =
                await _externalHttpClientWrapper
                    .GetFromJsonAsync<ApiResponse<List<GetDepartmentResponseDto>>>(
                        ApiRelativePaths.DEPARTMENT_GET_ALL,
                        cancellationToken);

            return apiResponse;
        }

        public async Task<ApiResponse<GetDepartmentResponseDto>> GetByIdAsync(
            long id,
            CancellationToken cancellationToken = default)
        {
            var apiResponse =
                await _externalHttpClientWrapper
                    .GetFromJsonAsync<ApiResponse<GetDepartmentResponseDto>>(
                        $"{ApiRelativePaths.DEPARTMENT_GET_BY_ID}?{nameof(id)}={id}",
                        cancellationToken);

            return apiResponse;
        }
    }
}
