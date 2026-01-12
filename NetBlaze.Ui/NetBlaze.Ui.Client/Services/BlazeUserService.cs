using NetBlaze.SharedKernel.Dtos.User.Requests;
using NetBlaze.SharedKernel.Dtos.User.Responses;
using NetBlaze.SharedKernel.Dtos.Vacation.Requests;
using NetBlaze.SharedKernel.HelperUtilities.Constants;
using NetBlaze.SharedKernel.HelperUtilities.General;
using NetBlaze.Ui.Client.InternalHelperTypes.General;
using NetBlaze.Ui.Client.Services.CommonServices;
using static System.Net.WebRequestMethods;

namespace NetBlaze.Ui.Client.Services
{
    public class BlazeUserService : BaseBlazService
    {
        public BlazeUserService(ExternalHttpClientWrapper externalHttpClientWrapper,
            CentralizedSnackbarProvider centralizedSnackbarProvider) : base(externalHttpClientWrapper, centralizedSnackbarProvider) { }

        public async Task<ApiResponse<List<GetManagerResponseDto>>> GetManagersAsync(CancellationToken cancellationToken = default)
        {
            var apiResponse = await _externalHttpClientWrapper.GetFromJsonAsync<ApiResponse<List<GetManagerResponseDto>>>
                (ApiRelativePaths.USER_GET_MANAGERS, cancellationToken);
            return apiResponse;
        }

        public async Task<ApiResponse<List<GetEmployeeResponseDto>>> GetEmployeesAsync(CancellationToken cancellationToken = default)
        {
            var apiResponse = await _externalHttpClientWrapper.GetFromJsonAsync<ApiResponse<List<GetEmployeeResponseDto>>>
                (ApiRelativePaths.USER_GET_EMPLOYEES, cancellationToken);
            return apiResponse;
        }

        public async Task<ApiResponse<PaginatedList<GetUserResponseDto>>> GetAllUsersAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
        {
            var url = $"{ApiRelativePaths.USER_GET_ALL}?pageNumber={pageNumber}&pageSize={pageSize}";

            return await _externalHttpClientWrapper.GetFromJsonAsync<ApiResponse<PaginatedList<GetUserResponseDto>>>(url, cancellationToken);
        }


        public async Task<ApiResponse<object>> DeleteUserAsync(long id, CancellationToken cancellationToken = default)
        {
            var apiResponse = await _externalHttpClientWrapper.DeleteFromJsonAsync<ApiResponse<object>>(
                    $"{ApiRelativePaths.USER_DELETE}/{id}",
                    cancellationToken);

            _centralizedSnackbarProvider.ShowApiResponseSnackbar(apiResponse);
            return apiResponse;
        }

        public async Task<ApiResponse<object>> UpdateUserAsync(UpdateUserRequestDto updateUserRequestDto, CancellationToken cancellationToken = default)
        {
            var apiResponse = await _externalHttpClientWrapper.PutAsJsonAsync<UpdateUserRequestDto, ApiResponse<object>>(ApiRelativePaths.USER_UPDATE, updateUserRequestDto, cancellationToken);

            _centralizedSnackbarProvider.ShowApiResponseSnackbar(apiResponse);

            return apiResponse;
        }

        public async Task<ApiResponse<object>> EditUserProfileAsync(EditProfileRequestDto editProfileRequestDto)
        {
            var apiResponse = await _externalHttpClientWrapper.PutAsJsonAsync<EditProfileRequestDto, ApiResponse<object>>(ApiRelativePaths.USER_EDIT_PROFILE, editProfileRequestDto);
            _centralizedSnackbarProvider.ShowApiResponseSnackbar(apiResponse);
            return apiResponse;
        }
        public async Task<ApiResponse<GetUserProfileResponseDto>> GetCurrentUserProfileAsync()
        {
            var apiResponse = await _externalHttpClientWrapper.GetFromJsonAsync<ApiResponse<GetUserProfileResponseDto>>(ApiRelativePaths.USER_GET_CURRENT_PROFILE);
            return apiResponse;
        }
    }
}
