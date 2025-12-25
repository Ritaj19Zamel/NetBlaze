using NetBlaze.Application.Mappings;
using NetBlaze.SharedKernel.Dtos.General;
using NetBlaze.SharedKernel.Dtos.Vacation.Requests;
using NetBlaze.SharedKernel.Dtos.Vacation.Responses;
using NetBlaze.SharedKernel.HelperUtilities.General;

namespace NetBlaze.Application.Interfaces.ServicesInterfaces
{
    public interface IVacationService
    {
        Task<ApiResponse<object>> CreateAsync(CreateVacationRequestDto createVacationRequestDto, CancellationToken cancellationToken = default);
        Task<ApiResponse<PaginatedList<GetVacationResponseDto>>> GetAllAsync(int PageNumber, int PageSize, CancellationToken cancellationToken = default);
        Task<ApiResponse<GetVacationResponseDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default);
        Task<ApiResponse<object>> UpdateAsync(long id, UpdateVacationRequestDto updateVacationRequestDto, CancellationToken cancellationToken = default);
        Task<ApiResponse<object>> DeleteAsync(long id, CancellationToken cancellationToken = default);
    }
}
