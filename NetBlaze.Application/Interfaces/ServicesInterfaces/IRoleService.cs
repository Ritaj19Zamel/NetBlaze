using NetBlaze.SharedKernel.Dtos.Role.Responses;
using NetBlaze.SharedKernel.HelperUtilities.General;

namespace NetBlaze.Application.Interfaces.ServicesInterfaces
{
    public interface IRoleService
    {
        Task<ApiResponse<List<GetRoleResponseDto>>> GetAllRolesAsync(CancellationToken cancellationToken = default);
        Task<ApiResponse<GetRoleResponseDto>> GetRolesByIdAsync(long id, CancellationToken cancellationToken = default);
    }
}
