using NetBlaze.Application.Mappings;
using NetBlaze.SharedKernel.Dtos.General;
using NetBlaze.SharedKernel.Dtos.Vacation.Requests;
using NetBlaze.SharedKernel.Dtos.Vacation.Responses;
using NetBlaze.SharedKernel.HelperUtilities.General;

namespace NetBlaze.Application.Interfaces.ServicesInterfaces
{
    public interface IVacationService
    {
        Task<ApiResponse<object>> CreateVacationAsync(CreateVacationRequestDto createVacationRequestDto, CancellationToken cancellationToken = default);
        Task<ApiResponse<PaginatedList<GetVacationResponseDto>>> GetAllVacationAsync(int PageNumber, int PageSize, CancellationToken cancellationToken = default);
        Task<ApiResponse<GetVacationResponseDto>> GetVacationByIdAsync(long id, CancellationToken cancellationToken = default);
        Task<ApiResponse<object>> UpdateVacationAsync(UpdateVacationRequestDto updateVacationRequestDto, CancellationToken cancellationToken = default);
        Task<ApiResponse<object>> DeleteVacationAsync(long id, CancellationToken cancellationToken = default);
    }
}
