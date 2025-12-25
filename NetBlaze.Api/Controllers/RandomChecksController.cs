using Microsoft.AspNetCore.Mvc;
using NetBlaze.Api.Filters;
using NetBlaze.Application.Interfaces.ServicesInterfaces;
using NetBlaze.Application.Mappings;
using NetBlaze.SharedKernel.Dtos.RandomCheck.Requests;
using NetBlaze.SharedKernel.Dtos.RandomCheck.Responses;
using NetBlaze.SharedKernel.HelperUtilities.General;

namespace NetBlaze.Api.Controllers
{
    [DynamicAuthorize]

    public class RandomChecksController : BaseNetBlazeController, IRandomChecksService
    {
        private readonly IRandomChecksService _randomChecksService;
        public RandomChecksController(IRandomChecksService randomChecksService)
        {
            _randomChecksService = randomChecksService;
        }

        [HttpPost("generateotp")]
        public async Task<ApiResponse<object>> GenerateOTP(GenerateOtpRequestDto generateOtpRequestDto, CancellationToken cancellationToken = default)
        {
            return await _randomChecksService.GenerateOTP(generateOtpRequestDto, cancellationToken);
        }
        [HttpGet("getuserchecks")]
        public async Task<ApiResponse<PaginatedList<GetUserRandomChecksResponseDto>>> GetUserChecksAsync([FromQuery]GetUserRandomChecksRequestDto getUserRandomChecksRequestDto, CancellationToken cancellationToken = default)
        {
           return await _randomChecksService.GetUserChecksAsync(getUserRandomChecksRequestDto, cancellationToken);
        }

        [HttpPost("verifyotp")]
        public async Task<ApiResponse<object>> VerifyOTP(VerifyOTPRequestDto verifyOTPRequestDto, CancellationToken cancellationToken = default)
        {
            return await _randomChecksService.VerifyOTP(verifyOTPRequestDto, cancellationToken);
        }
    }
}
