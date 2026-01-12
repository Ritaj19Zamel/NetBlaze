
using NetBlaze.Application.Mappings;
using NetBlaze.SharedKernel.Dtos.General;
using NetBlaze.SharedKernel.Dtos.Policy.Requests;
using NetBlaze.SharedKernel.Dtos.Policy.Responses;
using NetBlaze.SharedKernel.HelperUtilities.General;

namespace NetBlaze.Application.Interfaces.ServicesInterfaces
{
    public interface IPolicyService
    {
        Task<ApiResponse<object>> CreatePolicyAsync(CreatePolicyRequestDto createPolicyRequestDto, CancellationToken cancellationToken = default);
        Task<ApiResponse<GetPolicyResponseDto>> GetPolicyByIdAsync(long id, CancellationToken cancellationToken = default);
        Task<ApiResponse<PaginatedList<GetPolicyResponseDto>>> GetAllPoliciesAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default);
        Task<ApiResponse<object>> UpdatePolicyAsync(UpdatePolicyRequestDto updatePolicyRequestDto, CancellationToken cancellationToken = default);
        Task<ApiResponse<object>> DeletePolicyAsync(long id, CancellationToken cancellationToken = default);
       
    }
}
