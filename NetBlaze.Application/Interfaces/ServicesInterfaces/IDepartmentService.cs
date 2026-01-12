
using NetBlaze.Application.Mappings;
using NetBlaze.SharedKernel.Dtos.Department.Requests;
using NetBlaze.SharedKernel.Dtos.Department.Responses;
using NetBlaze.SharedKernel.HelperUtilities.General;

namespace NetBlaze.Application.Interfaces.ServicesInterfaces
{
    public interface IDepartmentService
    {
        Task<ApiResponse<List<GetDepartmentResponseDto>>> GetAllDepartmentAsync(CancellationToken cancellationToken = default);
        Task<ApiResponse<GetDepartmentResponseDto>> GetDepartmentByIdAsync(long id, CancellationToken cancellationToken = default);
        Task<ApiResponse<object>> CreateDepartmentAsync(CreateDepartmentRequestDto dto, CancellationToken cancellationToken = default);
        Task<ApiResponse<object>> UpdateDepartmentAsync(UpdateDepartmentRequestDto dto, CancellationToken cancellationToken = default);
        Task<ApiResponse<object>> DeleteDepartmentAsync(long id, CancellationToken cancellationToken = default);
        Task<ApiResponse<object>> ToggleDepartmentStatusAsync(long id, CancellationToken cancellationToken = default);
    }
}
