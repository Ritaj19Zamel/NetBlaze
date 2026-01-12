using NetBlaze.SharedKernel.Dtos.Vacation.Requests;
using NetBlaze.SharedKernel.Dtos.Vacation.Responses;
using NetBlaze.SharedKernel.HelperUtilities.Constants;
using NetBlaze.SharedKernel.HelperUtilities.General;
using NetBlaze.Ui.Client.InternalHelperTypes.General;
using NetBlaze.Ui.Client.Services.CommonServices;


namespace NetBlaze.Ui.Client.Services
{
    public class BlazeVacationService : BaseBlazService
    {
        public BlazeVacationService(ExternalHttpClientWrapper externalHttpClientWrapper, CentralizedSnackbarProvider centralizedSnackbarProvider) : base(externalHttpClientWrapper, centralizedSnackbarProvider) { }

        public async Task<ApiResponse<object>> CreateVacationAsync(CreateVacationRequestDto createVacationRequestDto, CancellationToken cancellationToken = default)
        {
            var apiResponse = await _externalHttpClientWrapper.PostAsJsonAsync<CreateVacationRequestDto, ApiResponse<object>>(ApiRelativePaths.VACATION_CREATE, createVacationRequestDto, cancellationToken);

            _centralizedSnackbarProvider.ShowApiResponseSnackbar(apiResponse);

            return apiResponse;
        }

        public async Task<PaginatedList<GetVacationResponseDto>> GetVacationsPagedAsync(
                            int pageNumber,
                            int pageSize,
                            CancellationToken cancellationToken = default)
        {
            var response =
                await _externalHttpClientWrapper.GetFromJsonAsync<
                    ApiResponse<PaginatedList<GetVacationResponseDto>>>(
                        $"{ApiRelativePaths.VACATION_GET_ALL}?PageNumber={pageNumber}&PageSize={pageSize}",
                        cancellationToken);

            return response.Success
                ? response.Data
                : new PaginatedList<GetVacationResponseDto>();
        }



        public async Task<ApiResponse<GetVacationResponseDto>> GetVacationAsync(long id, CancellationToken cancellationToken = default)
        {
            var apiResponse = await _externalHttpClientWrapper.GetFromJsonAsync<ApiResponse<GetVacationResponseDto>>(
                $"{ApiRelativePaths.VACATION_GET_BY_ID}/{id}", cancellationToken);

            return apiResponse;
        }

        public async Task<ApiResponse<object>> UpdateVacationAsync(UpdateVacationRequestDto updateVacationRequestDto, CancellationToken cancellationToken = default)
        {
            var apiResponse = await _externalHttpClientWrapper.PutAsJsonAsync<UpdateVacationRequestDto, ApiResponse<object>>(ApiRelativePaths.VACATION_UPDATE, updateVacationRequestDto, cancellationToken);

            _centralizedSnackbarProvider.ShowApiResponseSnackbar(apiResponse);

            return apiResponse;
        }

        public async Task<ApiResponse<object>> DeleteVacationAsync(long id, CancellationToken cancellationToken = default)
        {
            var apiResponse =
                await _externalHttpClientWrapper.DeleteFromJsonAsync<ApiResponse<object>>(
                    $"{ApiRelativePaths.VACATION_DELETE}/{id}",
                    cancellationToken);

            _centralizedSnackbarProvider.ShowApiResponseSnackbar(apiResponse);
            return apiResponse;
        }


    }
}
