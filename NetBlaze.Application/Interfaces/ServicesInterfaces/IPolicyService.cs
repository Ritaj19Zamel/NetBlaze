
using NetBlaze.SharedKernel.Dtos.Policy.Requests;
using NetBlaze.SharedKernel.Dtos.Policy.Responses;
using NetBlaze.SharedKernel.HelperUtilities.General;

namespace NetBlaze.Application.Interfaces.ServicesInterfaces
{
    public interface IPolicyService
    {
        Task<ApiResponse<object>> CreateAsync(CreatePolicyRequestDto createPolicyRequestDto, CancellationToken cancellationToken = default);
        Task<ApiResponse<GetPolicyResponseDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default);
        Task<ApiResponse<List<GetPolicyResponseDto>>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<ApiResponse<object>> UpdateAsync(long id, UpdatePolicyRequestDto updatePolicyRequestDto, CancellationToken cancellationToken = default);
        Task<ApiResponse<object>> DeleteAsync(long id, CancellationToken cancellationToken = default);
       
    }
}
