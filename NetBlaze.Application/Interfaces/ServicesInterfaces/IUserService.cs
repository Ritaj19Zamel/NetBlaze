using NetBlaze.SharedKernel.Dtos.User.Requests;
using NetBlaze.SharedKernel.Dtos.User.Responses;
using NetBlaze.SharedKernel.HelperUtilities.General;

namespace NetBlaze.Application.Interfaces.ServicesInterfaces
{
    public interface IUserService
    {
        Task<ApiResponse<List<GetManagerResponseDto>>> GetManagersAsync(CancellationToken cancellationToken = default);
        Task<ApiResponse<object>> UpdateUserAsync(UpdateUserRequestDto updateUserRequestDto, CancellationToken cancellationToken = default);


    }
}
