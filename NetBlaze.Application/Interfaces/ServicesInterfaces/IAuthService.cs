using NetBlaze.SharedKernel.Dtos.Auth.Requests;
using NetBlaze.SharedKernel.Dtos.Auth.Responses;
using NetBlaze.SharedKernel.Dtos.Fido;
using NetBlaze.SharedKernel.HelperUtilities.General;

namespace NetBlaze.Application.Interfaces.ServicesInterfaces
{
    public interface IAuthService
    {
        Task<ApiResponse<RegisterResponseDto>> RegisterAsync(RegisterRequestDto registerRequestDto, CancellationToken cancellationToken = default);
        Task<ApiResponse<LoginResponseDto>> LoginAsync(LoginRequestDto loginRequestDto, CancellationToken cancellationToken = default);
        Task<ApiResponse<object>> ForgetPasswordAsync(ForgetPasswordRequestDto forgetPasswordRequestDto);
        Task<ApiResponse<object>> ResetPasswordAsync(ResetPasswordRequestDto resetPasswordRequestDto);



    }
}
