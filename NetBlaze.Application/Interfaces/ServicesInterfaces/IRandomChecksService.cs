using NetBlaze.Application.Mappings;
using NetBlaze.SharedKernel.Dtos.RandomCheck.Requests;
using NetBlaze.SharedKernel.Dtos.RandomCheck.Responses;
using NetBlaze.SharedKernel.HelperUtilities.General;

namespace NetBlaze.Application.Interfaces.ServicesInterfaces
{
    public interface IRandomChecksService
    {
        Task<ApiResponse<object>> VerifyOTP(VerifyOTPRequestDto verifyOTPRequestDto, CancellationToken cancellationToken = default);
        Task<ApiResponse<object>> GenerateOTP(GenerateOtpRequestDto generateOtpRequestDto, CancellationToken cancellationToken = default);

        Task<ApiResponse<PaginatedList<GetUserRandomChecksResponseDto>>> GetUserChecksAsync(GetUserRandomChecksRequestDto getUserRandomChecksRequestDto,
            CancellationToken cancellationToken = default);
        Task<ApiResponse<object>> SaveAutoRandomCheckConfig(AutoRandomCheckRequestDto dto, CancellationToken cancellationToken);

    }
}
