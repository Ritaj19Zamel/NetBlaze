using NetBlaze.SharedKernel.Dtos.Vacation.Requests;
using NetBlaze.SharedKernel.Dtos.Vacation.Responses;
using NetBlaze.SharedKernel.HelperUtilities.Constants;
using NetBlaze.SharedKernel.HelperUtilities.General;
using NetBlaze.Ui.Client.Services.CommonServices;

namespace NetBlaze.Ui.Client.Services
{
    public class BlazVacationService : BaseBlazService
    {
        public BlazVacationService(
            ExternalHttpClientWrapper externalHttpClientWrapper,
            CentralizedSnackbarProvider centralizedSnackbarProvider)
            : base(externalHttpClientWrapper, centralizedSnackbarProvider) { }

        public async Task<ApiResponse<object>> CreateAsync(
            CreateVacationRequestDto createVacationRequestDto,
            CancellationToken cancellationToken = default)
        {
            var apiResponse =
                await _externalHttpClientWrapper
                    .PostAsJsonAsync<CreateVacationRequestDto, ApiResponse<object>>(
                        ApiRelativePaths.VACATION_CREATE,
                        createVacationRequestDto,
                        cancellationToken);

            _centralizedSnackbarProvider.ShowApiResponseSnackbar(apiResponse);

            return apiResponse;
        }

        public async Task<ApiResponse<List<GetVacationResponseDto>>> GetAllAsync(
            CancellationToken cancellationToken = default)
        {
            var apiResponse =
                await _externalHttpClientWrapper
                    .GetFromJsonAsync<ApiResponse<List<GetVacationResponseDto>>>(
                        ApiRelativePaths.VACATION_GET_ALL,
                        cancellationToken);

            return apiResponse;
        }

        public async Task<ApiResponse<GetVacationResponseDto>> GetByIdAsync(
            long id,
            CancellationToken cancellationToken = default)
        {
            var apiResponse =
                await _externalHttpClientWrapper
                    .GetFromJsonAsync<ApiResponse<GetVacationResponseDto>>(
                        $"{ApiRelativePaths.VACATION_GET_BY_ID}?{nameof(id)}={id}",
                        cancellationToken);

            return apiResponse;
        }

        public async Task<ApiResponse<object>> UpdateAsync(
            long id,
            UpdateVacationRequestDto updateVacationRequestDto,
            CancellationToken cancellationToken = default)
        {
            var apiResponse =
                await _externalHttpClientWrapper
                    .PutAsJsonAsync<UpdateVacationRequestDto, ApiResponse<object>>(
                        $"{ApiRelativePaths.VACATION_UPDATE}?{nameof(id)}={id}",
                        updateVacationRequestDto,
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
                        $"{ApiRelativePaths.VACATION_DELETE}?{nameof(id)}={id}",
                        cancellationToken);

            _centralizedSnackbarProvider.ShowApiResponseSnackbar(apiResponse);

            return apiResponse;
        }
    }
}
