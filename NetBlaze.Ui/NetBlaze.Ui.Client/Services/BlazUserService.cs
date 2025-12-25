using NetBlaze.SharedKernel.Dtos;
using NetBlaze.SharedKernel.Dtos.User.Requests;
using NetBlaze.SharedKernel.Dtos.User.Responses;
using NetBlaze.SharedKernel.HelperUtilities.Constants;
using NetBlaze.SharedKernel.HelperUtilities.General;
using NetBlaze.Ui.Client.Services.CommonServices;

namespace NetBlaze.Ui.Client.Services
{
    public class BlazUserService : BaseBlazService
    {
        public BlazUserService(
            ExternalHttpClientWrapper externalHttpClientWrapper,
            CentralizedSnackbarProvider centralizedSnackbarProvider)
            : base(externalHttpClientWrapper, centralizedSnackbarProvider) { }

        public async Task<ApiResponse<List<GetManagerResponseDto>>> GetManagersAsync(
            CancellationToken cancellationToken = default)
        {
            var apiResponse =
                await _externalHttpClientWrapper
                    .GetFromJsonAsync<ApiResponse<List<GetManagerResponseDto>>>(
                        ApiRelativePaths.USER_GET_MANAGERS,
                        cancellationToken);

            return apiResponse;
        }

        public async Task<ApiResponse<object>> UpdateUserAsync(
            UpdateUserRequestDto updateUserRequestDto,
            CancellationToken cancellationToken = default)
        {
            var apiResponse =
                await _externalHttpClientWrapper
                    .PutAsJsonAsync<UpdateUserRequestDto, ApiResponse<object>>(
                        ApiRelativePaths.USER_UPDATE,
                        updateUserRequestDto,
                        cancellationToken);

            _centralizedSnackbarProvider.ShowApiResponseSnackbar(apiResponse);

            return apiResponse;
        }
        public async Task<ApiResponse<object>> AddUserAsync(
    AddUserRequestDto dto,
    CancellationToken cancellationToken = default)
        {
            var response =
                await _externalHttpClientWrapper
                    .PostAsJsonAsync<AddUserRequestDto, ApiResponse<object>>(
                        ApiRelativePaths.USER_ADD,
                        dto,
                        cancellationToken);

            _centralizedSnackbarProvider.ShowApiResponseSnackbar(response);
            return response;
        }

    }
}
