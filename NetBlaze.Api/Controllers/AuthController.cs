using Microsoft.AspNetCore.Mvc;
using NetBlaze.Application.Interfaces.ServicesInterfaces;
using NetBlaze.SharedKernel.Dtos.Auth.Requests;
using NetBlaze.SharedKernel.Dtos.Auth.Responses;
using NetBlaze.SharedKernel.HelperUtilities.General;

namespace NetBlaze.Api.Controllers
{
    public class AuthController : BaseNetBlazeController, IAuthService
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        [HttpPost("register")]
        public async Task<ApiResponse<RegisterResponseDto>> RegisterAsync(RegisterRequestDto registerRequestDto, CancellationToken cancellationToken = default)
        {
            return await _authService.RegisterAsync(registerRequestDto, cancellationToken);
        }
        [HttpPost("login")]
        public async Task<ApiResponse<LoginResponseDto>> LoginAsync(LoginRequestDto loginRequestDto, CancellationToken cancellationToken = default)
        {
            return await _authService.LoginAsync(loginRequestDto, cancellationToken);
        }
        [HttpPost("forgetpassword")]
        public async Task<ApiResponse<object>> ForgetPasswordAsync(ForgetPasswordRequestDto forgetPasswordRequestDto)
        {
            return await _authService.ForgetPasswordAsync(forgetPasswordRequestDto);
        }

        [HttpPost("resetpassword")]
        public async Task<ApiResponse<object>> ResetPasswordAsync(ResetPasswordRequestDto resetPasswordRequestDto)
        {
            return await _authService.ResetPasswordAsync(resetPasswordRequestDto);
        }


    }
}
