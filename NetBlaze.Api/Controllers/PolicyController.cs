
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetBlaze.Application.Interfaces.ServicesInterfaces;
using NetBlaze.Application.Mappings;
using NetBlaze.SharedKernel.Dtos.Policy.Requests;
using NetBlaze.SharedKernel.Dtos.Policy.Responses;
using NetBlaze.SharedKernel.HelperUtilities.General;

namespace NetBlaze.Api.Controllers
{
    [Authorize(Policy = AuthorizationPolicies.HRAndManager)]
    public class PolicyController : BaseNetBlazeController, IPolicyService
    {
        private readonly IPolicyService _policyService;
        public PolicyController(IPolicyService policyService)
        {
           _policyService = policyService;
        }
        [HttpPost("CreatePolicy")]
        public async Task<ApiResponse<object>> CreatePolicyAsync(CreatePolicyRequestDto createPolicyRequestDto
            , CancellationToken cancellationToken = default)
        {
            return await _policyService.CreatePolicyAsync(createPolicyRequestDto , cancellationToken);
        }
        

        [HttpGet("GetPolicyById")]
        public async Task<ApiResponse<GetPolicyResponseDto>> GetPolicyByIdAsync(long id, CancellationToken cancellationToken = default)
        {
            return await _policyService.GetPolicyByIdAsync(id, cancellationToken);
        }
        [HttpPut("UpdatePolicy")]
        public async Task<ApiResponse<object>> UpdatePolicyAsync(UpdatePolicyRequestDto updatePolicyRequestDto, CancellationToken cancellationToken = default)
        {
            return await _policyService.UpdatePolicyAsync(updatePolicyRequestDto, cancellationToken);
        }
        [HttpDelete("DeletePolicy")]

        public async Task<ApiResponse<object>> DeletePolicyAsync(long id, CancellationToken cancellationToken = default)
        {
            return await _policyService.DeletePolicyAsync(id, cancellationToken);
        }
        [HttpGet("GetPolicies")]
        public async Task<ApiResponse<PaginatedList<GetPolicyResponseDto>>> GetAllPoliciesAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
        {
            return await _policyService.GetAllPoliciesAsync(pageNumber, pageSize, cancellationToken);
        }
    }
}
