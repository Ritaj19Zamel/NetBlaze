using NetBlaze.SharedKernel.Dtos.Vacation.Requests;
using NetBlaze.SharedKernel.Dtos.Vacation.Responses;
using NetBlaze.SharedKernel.HelperUtilities.General;

namespace NetBlaze.Application.Interfaces.ServicesInterfaces
{
    public interface IVacationService
    {
        Task<ApiResponse<string>> CreateAsync(CreateVacationRequestDto createVacationRequestDto, CancellationToken cancellationToken = default);
        Task<ApiResponse<List<GetVacationResponseDto>>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<ApiResponse<GetVacationResponseDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default);
        Task<ApiResponse<string>> UpdateAsync(long id, UpdateVacationRequestDto updateVacationRequestDto, CancellationToken cancellationToken = default);
        Task<ApiResponse<string>> DeleteAsync(long id, CancellationToken cancellationToken = default);
    }
}
