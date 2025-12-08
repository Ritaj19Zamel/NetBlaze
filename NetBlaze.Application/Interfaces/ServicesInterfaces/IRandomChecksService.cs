using NetBlaze.SharedKernel.Dtos.OTP.Requests;
using NetBlaze.SharedKernel.HelperUtilities.General;

namespace NetBlaze.Application.Interfaces.ServicesInterfaces
{
    public interface IRandomChecksService
    {
        Task<ApiResponse<object>> VerifyOTP(VerifyOTPRequestDto verifyOTPRequestDto, CancellationToken cancellationToken = default);
        Task<ApiResponse<object>> GenerateOTP(GenerateOtpRequestDto generateOtpRequestDto, CancellationToken cancellationToken = default);

    }
}
