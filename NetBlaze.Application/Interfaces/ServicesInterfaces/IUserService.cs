using NetBlaze.Application.Mappings;
using NetBlaze.SharedKernel.Dtos.Auth.Requests;
using NetBlaze.SharedKernel.Dtos.User.Requests;
using NetBlaze.SharedKernel.Dtos.User.Responses;
using NetBlaze.SharedKernel.HelperUtilities.General;

namespace NetBlaze.Application.Interfaces.ServicesInterfaces
{
    public interface IUserService
    {
        Task<ApiResponse<List<GetManagerResponseDto>>> GetManagersAsync(CancellationToken cancellationToken = default);
        
        Task<ApiResponse<object>> UpdateUserAsync(UpdateUserRequestDto updateUserRequestDto, CancellationToken cancellationToken = default);
        
        Task<ApiResponse<List<GetEmployeeResponseDto>>> GetEmployeesAsync(CancellationToken cancellationToken = default);

        Task<ApiResponse<PaginatedList<GetUserResponseDto>>> GetAllUsersAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default);

        Task<ApiResponse<object>> DeleteUserAsync(long id, CancellationToken cancellationToken = default);

        Task<ApiResponse<object>> EditUserProfileAsync(EditProfileRequestDto editProfileRequestDto);

        Task<ApiResponse<GetUserProfileResponseDto>> GetCurrentUserProfileAsync();


    }
}
