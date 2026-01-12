

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetBlaze.Application.Interfaces.ServicesInterfaces;
using NetBlaze.Application.Mappings;
using NetBlaze.Domain.Entities;
using NetBlaze.SharedKernel.Dtos.User.Requests;
using NetBlaze.SharedKernel.Dtos.User.Responses;
using NetBlaze.SharedKernel.HelperUtilities.General;

namespace NetBlaze.Api.Controllers
{
    
    public class UserController : BaseNetBlazeController, IUserService
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [Authorize(Policy = AuthorizationPolicies.HRAndManager)]
        [HttpDelete("DeleteUser/{id:long}")]
        public async Task<ApiResponse<object>> DeleteUserAsync(long id, CancellationToken cancellationToken = default)
        {
            return await _userService.DeleteUserAsync(id, cancellationToken);
        }

        [Authorize(Policy = AuthorizationPolicies.AnyAuthenticated)]
        [HttpPut("EditUserProfile")]
        public async Task<ApiResponse<object>> EditUserProfileAsync(EditProfileRequestDto editProfileRequestDto)
        {
            return await _userService.EditUserProfileAsync(editProfileRequestDto);
        }

        [Authorize(Policy = AuthorizationPolicies.HRAndManager)]
        [HttpGet("GetAllUsers")]
        public async Task<ApiResponse<PaginatedList<GetUserResponseDto>>> GetAllUsersAsync([FromQuery] int pageNumber, [FromQuery] int pageSize,
                    CancellationToken cancellationToken = default)
        {
            return await _userService.GetAllUsersAsync(pageNumber, pageSize, cancellationToken);
        }

        [Authorize(Policy = AuthorizationPolicies.AnyAuthenticated)]
        [HttpGet("GetCurrentUserProfile")]
        public async Task<ApiResponse<GetUserProfileResponseDto>> GetCurrentUserProfileAsync()
        {
            return await _userService.GetCurrentUserProfileAsync();
        }

        [Authorize(Policy = AuthorizationPolicies.HRAndManager)]
        [HttpGet("GetEmployees")]
        public async Task<ApiResponse<List<GetEmployeeResponseDto>>> GetEmployeesAsync(CancellationToken cancellationToken = default)
        {
            return await _userService.GetEmployeesAsync(cancellationToken);
        }

        [Authorize(Policy = AuthorizationPolicies.HRAndManager)]
        [HttpGet("GetManagers")]
        public async Task<ApiResponse<List<GetManagerResponseDto>>> GetManagersAsync(CancellationToken cancellationToken = default)
        {
            return await _userService.GetManagersAsync(cancellationToken);
        }

        [Authorize(Policy = AuthorizationPolicies.HRAndManager)]
        [HttpPut("UpdateUser")]
        public async Task<ApiResponse<object>> UpdateUserAsync(UpdateUserRequestDto updateUserRequestDto, CancellationToken cancellationToken = default)
        {
            return await _userService.UpdateUserAsync(updateUserRequestDto, cancellationToken);
        }
    }
}
