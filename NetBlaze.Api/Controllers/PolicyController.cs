
using Microsoft.AspNetCore.Mvc;
using NetBlaze.Application.Interfaces.ServicesInterfaces;
using NetBlaze.SharedKernel.Dtos.Policy.Requests;
using NetBlaze.SharedKernel.Dtos.Policy.Responses;
using NetBlaze.SharedKernel.HelperUtilities.General;

namespace NetBlaze.Api.Controllers
{

    public class PolicyController : BaseNetBlazeController, IPolicyService
    {
        private readonly IPolicyService _policyService;
        public PolicyController(IPolicyService policyService)
        {
           _policyService = policyService;
        }
        [HttpPost("")]
        public async Task<ApiResponse<string>> CreateAsync(CreatePolicyRequestDto createPolicyRequestDto
            , CancellationToken cancellationToken = default)
        {
            return await _policyService.CreateAsync(createPolicyRequestDto , cancellationToken);
        }
        [HttpDelete("{id:long}")]

        public async Task<ApiResponse<string>> DeleteAsync(long id, CancellationToken cancellationToken = default)
        {
            return await _policyService.DeleteAsync(id, cancellationToken);
        }
        [HttpGet("policies")]

        public async Task<ApiResponse<List<GetPolicyResponseDto>>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _policyService.GetAllAsync(cancellationToken);
        }
        [HttpGet("{id:long}")]
        public async Task<ApiResponse<GetPolicyResponseDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default)
        {
            return await _policyService.GetByIdAsync(id, cancellationToken);
        }
        [HttpPut("{id:long}")]
        public async Task<ApiResponse<string>> UpdateAsync(long id, UpdatePolicyRequestDto updatePolicyRequestDto, CancellationToken cancellationToken = default)
        {
            return await _policyService.UpdateAsync(id, updatePolicyRequestDto, cancellationToken);
        }
    }
}
