using NetBlaze.SharedKernel.Dtos.Department.Requests;
using NetBlaze.SharedKernel.Dtos.Department.Responses;
using NetBlaze.SharedKernel.Dtos.Sample.Requests;
using NetBlaze.SharedKernel.HelperUtilities.Constants;
using NetBlaze.SharedKernel.HelperUtilities.General;
using NetBlaze.Ui.Client.Services.CommonServices;

namespace NetBlaze.Ui.Client.Services
{
    public class BlazeDepartmentService : BaseBlazService
    {
        public BlazeDepartmentService(ExternalHttpClientWrapper externalHttpClientWrapper, 
            CentralizedSnackbarProvider centralizedSnackbarProvider) : base(externalHttpClientWrapper, centralizedSnackbarProvider) { }

        public async Task<ApiResponse<List<GetDepartmentResponseDto>>> GetAllDepartmentAsync(CancellationToken cancellationToken = default)
        {
            var apiResponse = await _externalHttpClientWrapper.GetFromJsonAsync<ApiResponse<List<GetDepartmentResponseDto>>>
                (ApiRelativePaths.DEPARTMENT_GET_ALL, cancellationToken);
            return apiResponse;
        }
        
        public async Task<ApiResponse<GetDepartmentResponseDto>> GetDepartmentByIdAsync(long id, CancellationToken cancellationToken = default)
        {
            var apiResponse = await _externalHttpClientWrapper.GetFromJsonAsync<ApiResponse<GetDepartmentResponseDto>>
                ($"{ApiRelativePaths.DEPARTMENT_GET_BY_ID}/{id}", cancellationToken);
            return apiResponse;
        }

        public async Task<ApiResponse<object>> CreateDepartmentAsync(CreateDepartmentRequestDto createDepartmentRequestDto, CancellationToken cancellationToken = default)
        {
            var apiResponse = await _externalHttpClientWrapper.PostAsJsonAsync<CreateDepartmentRequestDto, ApiResponse<object>>(ApiRelativePaths.DEPARTMENT_CREATE, createDepartmentRequestDto, cancellationToken);

            _centralizedSnackbarProvider.ShowApiResponseSnackbar(apiResponse);

            return apiResponse;
        }

        public async Task<ApiResponse<object>> UpdateDepartmentAsync(UpdateDepartmentRequestDto updateDepartmentRequestDto, CancellationToken cancellationToken = default)
        {
            var apiResponse = await _externalHttpClientWrapper.PutAsJsonAsync<UpdateDepartmentRequestDto, ApiResponse<object>>(ApiRelativePaths.DEPARTMENT_UPDATE, updateDepartmentRequestDto, cancellationToken);

            _centralizedSnackbarProvider.ShowApiResponseSnackbar(apiResponse);

            return apiResponse;
        }

        public async Task<ApiResponse<object>> DeleteDepartmentAsync(long id, CancellationToken cancellationToken = default)
        {
            var apiResponse = await _externalHttpClientWrapper
                                .DeleteFromJsonAsync<ApiResponse<object>>(
                                    $"{ApiRelativePaths.DEPARTMENT_DELETE}/{id}",
                                    cancellationToken);

            _centralizedSnackbarProvider.ShowApiResponseSnackbar(apiResponse);

            return apiResponse;
        }

        public async Task<ApiResponse<object>> ToggleDepartmentStatusAsync(long id, CancellationToken cancellationToken = default)
        {
            var apiResponse = await _externalHttpClientWrapper.PostAsJsonAsync<object, ApiResponse<object>>(
                                $"{ApiRelativePaths.DEPARTMENT_TOGGLE_STATUS}/{id}",
                                new { },
                                cancellationToken);

            _centralizedSnackbarProvider.ShowApiResponseSnackbar(apiResponse);

            return apiResponse;
        }
    }
}
