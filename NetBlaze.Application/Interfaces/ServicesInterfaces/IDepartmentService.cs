
using NetBlaze.SharedKernel.Dtos.Department.Responses;
using NetBlaze.SharedKernel.HelperUtilities.General;

namespace NetBlaze.Application.Interfaces.ServicesInterfaces
{
    public interface IDepartmentService
    {
        Task<ApiResponse<List<GetDepartmentResponseDto>>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<ApiResponse<GetDepartmentResponseDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    }
}
