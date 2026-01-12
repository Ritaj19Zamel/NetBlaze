using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetBlaze.Application.Interfaces.ServicesInterfaces;
using NetBlaze.Application.Mappings;
using NetBlaze.SharedKernel.Dtos.RandomCheck.Requests;
using NetBlaze.SharedKernel.Dtos.RandomCheck.Responses;
using NetBlaze.SharedKernel.HelperUtilities.General;

namespace NetBlaze.Api.Controllers
{

    public class RandomChecksController : BaseNetBlazeController, IRandomChecksService
    {
        private readonly IRandomChecksService _randomChecksService;
        public RandomChecksController(IRandomChecksService randomChecksService)
        {
            _randomChecksService = randomChecksService;
        }

        [Authorize(Policy = AuthorizationPolicies.HRAndManager)]
        [HttpPost("GenerateOTP")]
        public async Task<ApiResponse<object>> GenerateOTP([FromBody] GenerateOtpRequestDto generateOtpRequestDto, CancellationToken cancellationToken = default)
        {
            return await _randomChecksService.GenerateOTP(generateOtpRequestDto, cancellationToken);
        }
        [HttpGet("GetUserChecks")]
        public async Task<ApiResponse<PaginatedList<GetUserRandomChecksResponseDto>>> GetUserChecksAsync([FromQuery]GetUserRandomChecksRequestDto getUserRandomChecksRequestDto, CancellationToken cancellationToken = default)
        {
           return await _randomChecksService.GetUserChecksAsync(getUserRandomChecksRequestDto, cancellationToken);
        }

        [Authorize(Policy = AuthorizationPolicies.HRAndManager)]
        [HttpPost("SaveAutoRandomCheckConfig")]
        public async Task<ApiResponse<object>> SaveAutoRandomCheckConfig(AutoRandomCheckRequestDto dto, CancellationToken cancellationToken)
        {
            return await _randomChecksService.SaveAutoRandomCheckConfig(dto, cancellationToken);
        }

        [HttpPost("VerifyOTP")]
        public async Task<ApiResponse<object>> VerifyOTP(VerifyOTPRequestDto verifyOTPRequestDto, CancellationToken cancellationToken = default)
        {
            return await _randomChecksService.VerifyOTP(verifyOTPRequestDto, cancellationToken);
        }
    }
}
