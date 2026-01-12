
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NetBlaze.Application.Interfaces.General;
using NetBlaze.Application.Interfaces.ServicesInterfaces;
using NetBlaze.Domain.Entities.Identity;
using NetBlaze.SharedKernel.Dtos.Auth.Requests;
using NetBlaze.SharedKernel.Dtos.Auth.Responses;
using NetBlaze.SharedKernel.Dtos.General;
using NetBlaze.SharedKernel.Enums;
using NetBlaze.SharedKernel.HelperUtilities.General;
using NetBlaze.SharedKernel.SharedResources;
using System.Net;

namespace NetBlaze.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IJwtBearerService _jwtBearerService;
        private readonly IEmailService _emailService;



        public AuthService(
                UserManager<User> userManager,
                RoleManager<Role> roleManager,
                IJwtBearerService jwtBearerService,
                SignInManager<User> signInManager,
                IEmailService emailService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtBearerService = jwtBearerService;
            _signInManager = signInManager;
            _emailService = emailService;
        }

        private static ApiResponse<RegisterResponseDto> HandleIdentityErrors(IdentityResult result)
        {
            var errorCodes = result.Errors.Select(e => e.Code).ToList();

            if (errorCodes.Contains("DuplicateEmail"))
                return ApiResponse<RegisterResponseDto>.ReturnFailureResponse(
                    Messages.EmailExists,
                    HttpStatusCode.BadRequest);

            if (errorCodes.Contains("DuplicateUserName"))
                return ApiResponse<RegisterResponseDto>.ReturnFailureResponse(
                    Messages.DuplicateUserName,
                    HttpStatusCode.BadRequest);

            if (errorCodes.Contains("InvalidUserName"))
                return ApiResponse<RegisterResponseDto>.ReturnFailureResponse(
                    Messages.DuplicateUserName,
                    HttpStatusCode.BadRequest);

            return ApiResponse<RegisterResponseDto>.ReturnFailureResponse(
                Messages.UserNotAdded,
                HttpStatusCode.BadRequest);
        }



        public async Task<ApiResponse<RegisterResponseDto>> RegisterAsync(RegisterRequestDto registerRequestDto, CancellationToken cancellationToken = default)
        {
            var user = new User
            {
                UserName = registerRequestDto.Email,
                DisplayName = registerRequestDto.DisplayName,
                PhoneNumber = registerRequestDto.PhoneNumber,
                Email = registerRequestDto.Email,
                DepartmentId = registerRequestDto.DepartmentId,
                ManagerId = registerRequestDto.ManagerId == 0 ? null : registerRequestDto.ManagerId
            };

            var result = await _userManager.CreateAsync(user, registerRequestDto.Password);

            if (!result.Succeeded)
            {
                return HandleIdentityErrors(result);
            }

            var role = await _roleManager.FindByIdAsync(registerRequestDto.RoleId.ToString());
            await _userManager.AddToRoleAsync(user, role!.Name!);

            return ApiResponse<RegisterResponseDto>.ReturnSuccessResponse(new RegisterResponseDto
            {
                UserId = user.Id,
                Email = user.Email!,
                DisplayName = user.DisplayName,
                DepartmentId = user.DepartmentId,
                RoleId = registerRequestDto.RoleId,
                ManagerId = user.ManagerId
            }, 
            Messages.UserAdded);


        }
        public async Task<ApiResponse<LoginResponseDto>> LoginAsync(LoginRequestDto loginRequestDto, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.Users
                                        .Include(u => u.UserDevices)
                                        .Include(u => u.UserRoles)
                                        .ThenInclude(ur => ur.Role)
                                        .FirstOrDefaultAsync(u => u.Email == loginRequestDto.Email, cancellationToken);
            if (user == null)
            {
                return ApiResponse<LoginResponseDto>.ReturnFailureResponse(Messages.InvalidCredentials, HttpStatusCode.Unauthorized);
            }
                
            var validPassword = await _signInManager.CheckPasswordSignInAsync(user, loginRequestDto.Password, true);

            if (validPassword.IsLockedOut)
            {
                return ApiResponse<LoginResponseDto>.ReturnFailureResponse(Messages.AccountLocked, HttpStatusCode.Forbidden);
            }
               
            if (!validPassword.Succeeded)
            {
                return ApiResponse<LoginResponseDto>.ReturnFailureResponse(Messages.InvalidCredentials, HttpStatusCode.Unauthorized);
            }


            var roles = user.UserRoles
                         .Select(ur => ur.Role.Name)
                         .Select(name => Enum.TryParse<AppRoles>(name, out var r) ? r : (AppRoles?)null)
                         .Where(r => r != null)
                         .Select(r => r!.Value)
                         .ToList();


            var tokenString = _jwtBearerService.GenerateToken(new GenerateTokenRequestDto(user.Id,user.UserName!,user.Email!,roles));

            var hasActiveDevice = user.UserDevices.Any(ud => ud.IsActive);

            var response = new LoginResponseDto
            {
                UserId = user.Id,
                DisplayName = user.DisplayName,
                Token = tokenString,
                RequiresDeviceRegistration = !hasActiveDevice,
                RequiresDeviceAuthentication = hasActiveDevice
            };

            return ApiResponse<LoginResponseDto>.ReturnSuccessResponse(response, Messages.LoginSuccess);
        }

        public async Task<ApiResponse<object>> ForgetPasswordAsync(ForgetPasswordRequestDto forgetPasswordRequestDto)
        {
            var user = await _userManager.FindByEmailAsync(forgetPasswordRequestDto.Email);

            if (user == null)
            {
                return ApiResponse<object>.ReturnFailureResponse(Messages.EmailNotExists, HttpStatusCode.BadRequest);
            }
                
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var encodedToken = WebUtility.UrlEncode(token);
            var resetUrl = $"{forgetPasswordRequestDto.ClientUrl}?email={forgetPasswordRequestDto.Email}&token={encodedToken}";
            var subject = "Reset Password";
            var body = $"Click the link to reset your password: {resetUrl}";

            await _emailService.SendAsync(forgetPasswordRequestDto.Email, subject, body);

            return ApiResponse<object>.ReturnSuccessResponse(Messages.ResetEmailSent, Messages.ResetEmailSent);
        }

        public async Task<ApiResponse<object>> ResetPasswordAsync(ResetPasswordRequestDto resetPasswordRequestDto)
        {
            var user = await _userManager.FindByEmailAsync(resetPasswordRequestDto.Email);

            if (user == null)
            {
                return ApiResponse<object>.ReturnFailureResponse(Messages.InvalidResetLink, HttpStatusCode.BadRequest);
            }

            var decodedToken = WebUtility.UrlDecode(resetPasswordRequestDto.Token);

            var result = await _userManager.ResetPasswordAsync(user, decodedToken, resetPasswordRequestDto.Password);

            if (!result.Succeeded)
            {
                return ApiResponse<object>.ReturnFailureResponse(Messages.PasswordResetFailed, HttpStatusCode.BadRequest);
            }
                

            return ApiResponse<object>.ReturnSuccessResponse(Messages.PasswordResetSuccess, Messages.PasswordResetSuccess);
        }








    }
}
