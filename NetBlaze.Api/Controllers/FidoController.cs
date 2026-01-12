
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetBlaze.Application.Interfaces.General;
using NetBlaze.SharedKernel.Dtos.Fido.Requests;
using NetBlaze.SharedKernel.Dtos.Fido.Responses;
using NetBlaze.SharedKernel.HelperUtilities.General;

namespace NetBlaze.Api.Controllers
{
    [Authorize(Policy = AuthorizationPolicies.Employee)]
    public class FidoController : BaseNetBlazeController, IFidoService
    {
        private readonly IFidoService _fido2;

        public FidoController(IFidoService fido2)
        {
            _fido2 = fido2;
        }

  
        [HttpPost("FidoRegisterOptions")]
        public async Task<ApiResponse<FidoRegisterOptionsResponseDto>> StartFidoRegisterAsync([FromBody] FidoLoginOptionsRequestDto request)
        {
            var result = await _fido2.StartFidoRegisterAsync(request);
            return result;
        }
        [HttpPost("FidoRegisterComplete")]
        public async Task<ApiResponse<object>> CompleteFidoRegisterAsync(FidoRegisterCompleteRequestDto dto)
        {
            var result = await _fido2.CompleteFidoRegisterAsync(dto);
            return result;
        }

        [HttpPost("FidoLoginOptions")]
        public async Task<ApiResponse<FidoLoginOptionsResponseDto>> StartFidoLoginAsync([FromBody] FidoLoginOptionsRequestDto request)
        {
            return await _fido2.StartFidoLoginAsync(request);
        }

        [HttpPost("FidoLoginComplete")]
        public async Task<ApiResponse<object>> CompleteFidoLoginAsync(FidoLoginCompleteRequestDto dto)
        {
            return await _fido2.CompleteFidoLoginAsync(dto);
        }
    }
}


