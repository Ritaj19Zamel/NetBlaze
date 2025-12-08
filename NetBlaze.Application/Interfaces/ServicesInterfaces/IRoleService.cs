using NetBlaze.SharedKernel.Dtos.Role.Responses;
using NetBlaze.SharedKernel.HelperUtilities.General;

namespace NetBlaze.Application.Interfaces.ServicesInterfaces
{
    public interface IRoleService
    {
        Task<ApiResponse<List<GetRoleResponseDto>>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<ApiResponse<GetRoleResponseDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    }
}
