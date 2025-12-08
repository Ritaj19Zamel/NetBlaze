using Microsoft.AspNetCore.Mvc;
using NetBlaze.Application.Interfaces.ServicesInterfaces;
using NetBlaze.SharedKernel.Dtos.OTP.Requests;
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

        [HttpPost("generateotp")]
        public async Task<ApiResponse<object>> GenerateOTP(GenerateOtpRequestDto generateOtpRequestDto, CancellationToken cancellationToken = default)
        {
            return await _randomChecksService.GenerateOTP(generateOtpRequestDto, cancellationToken);
        }
        [HttpPost("verifyotp")]

        public async Task<ApiResponse<object>> VerifyOTP(VerifyOTPRequestDto verifyOTPRequestDto, CancellationToken cancellationToken = default)
        {
            return await _randomChecksService.VerifyOTP(verifyOTPRequestDto, cancellationToken);
        }
    }
}
