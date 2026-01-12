using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NetBlaze.Application.Interfaces.General;
using NetBlaze.Application.Interfaces.ServicesInterfaces;
using NetBlaze.Application.Mappings;
using NetBlaze.Domain.Entities.Identity;
using NetBlaze.SharedKernel.Dtos.Auth.Requests;
using NetBlaze.SharedKernel.Dtos.User.Requests;
using NetBlaze.SharedKernel.Dtos.User.Responses;
using NetBlaze.SharedKernel.Enums;
using NetBlaze.SharedKernel.HelperUtilities.General;
using NetBlaze.SharedKernel.SharedResources;
using System.Net;

namespace NetBlaze.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserContext _userContext;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;

        public UserService(IUnitOfWork unitOfWork, IUserContext userContext, UserManager<User> userManager
            , RoleManager<Role> roleManager)
        {
            _unitOfWork = unitOfWork;
            _userContext = userContext;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<ApiResponse<List<GetManagerResponseDto>>> GetManagersAsync(CancellationToken cancellationToken = default)
        {
            var managers = await _unitOfWork.Repository.GetMultipleAsync<User, GetManagerResponseDto>(true,
                u => u.ManagerId == null,
                e => new GetManagerResponseDto()
                {
                    Id = e.Id,
                    Name = e.DisplayName
                }, cancellationToken);

            if(managers == null)
            {
                return ApiResponse<List<GetManagerResponseDto>>.ReturnFailureResponse(Messages.NoManagers, HttpStatusCode.NotFound);
            }
                
            return ApiResponse<List<GetManagerResponseDto>>.ReturnSuccessResponse(managers);

        }
        public async Task<ApiResponse<object>> UpdateUserAsync(UpdateUserRequestDto updateUserRequestDto, CancellationToken cancellationToken = default)
        {
           
                
            var user = await _userManager.Users
                .Include(u => u.UserRoles)
                .FirstOrDefaultAsync(u => u.Id == updateUserRequestDto.Id, cancellationToken);

            if(user == null)
            {
                return ApiResponse<object>.ReturnFailureResponse(Messages.UserNotFound, HttpStatusCode.NotFound);
            }
                
            user.DisplayName = updateUserRequestDto.DisplayName;
            user.DepartmentId = updateUserRequestDto.DepartmentId;
            user.ManagerId = updateUserRequestDto.ManagerId == 0 ? null : updateUserRequestDto.ManagerId;


            var currentRoles = await _userManager.GetRolesAsync(user);

            await _userManager.RemoveFromRolesAsync(user, currentRoles);

            var newRole = await _roleManager.FindByIdAsync(updateUserRequestDto.RoleId.ToString());

            await _userManager.AddToRoleAsync(user, newRole.Name);

            await _userManager.UpdateAsync(user);

            return ApiResponse<object>.ReturnSuccessResponse(Messages.UserUpdated, Messages.UserUpdated);

        }
        public async Task<ApiResponse<List<GetEmployeeResponseDto>>> GetEmployeesAsync(CancellationToken cancellationToken = default)
        {
            var role = await _roleManager.FindByNameAsync(AppRoles.Employee.ToString());

            if (role == null)
            {
                return ApiResponse<List<GetEmployeeResponseDto>>
                    .ReturnFailureResponse(Messages.RoleNotFound, HttpStatusCode.NotFound);
            }

            var usersInRole = await _userManager.GetUsersInRoleAsync(role.Name);

            if (usersInRole == null || !usersInRole.Any())
            {
                return ApiResponse<List<GetEmployeeResponseDto>>
                    .ReturnFailureResponse(Messages.NoEmployeesFound, HttpStatusCode.NotFound);
            }

            var result = usersInRole
                .Select(u => new GetEmployeeResponseDto
                {
                    Id = u.Id,
                    DisplayName = u.DisplayName
                })
                .ToList();

            return ApiResponse<List<GetEmployeeResponseDto>>
                .ReturnSuccessResponse(result);
        }

        public async Task<ApiResponse<PaginatedList<GetUserResponseDto>>>GetAllUsersAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
        {
            var users = _unitOfWork.Repository.GetQueryable<User>().AsNoTracking()
            .Where(u => !u.IsDeleted && u.IsActive)
            .Select(u => new GetUserResponseDto
            {
                Id = u.Id,
                UserName = u.UserName!,
                Email = u.Email!,
                DisplayName = u.DisplayName,
                Department = u.Department.DepartmentName,
                DepartmentId = u.DepartmentId,
                ManagerId = u.ManagerId,
                Role = u.UserRoles
                    .Where(ur => !ur.IsDeleted)
                    .Select(ur => ur.Role.Name!)
                    .FirstOrDefault()!,
                RoleId = u.UserRoles
                    .Where(ur => !ur.IsDeleted)
                    .Select(ur => ur.Role.Id)
                    .FirstOrDefault()

            })
            .OrderBy(u => u.UserName);


            var pagedResult = await users.PaginatedListAsync(pageNumber, pageSize);

            if (!pagedResult.Items.Any())
            {
                return ApiResponse<PaginatedList<GetUserResponseDto>>.ReturnFailureResponse(Messages.UserNotFound, HttpStatusCode.NotFound);
            }

            return ApiResponse<PaginatedList<GetUserResponseDto>>.ReturnSuccessResponse(pagedResult);
        }

        public async Task<ApiResponse<object>> DeleteUserAsync(long id, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                return ApiResponse<object>.ReturnFailureResponse(Messages.UserNotFound, HttpStatusCode.NotFound);
            }

            user.SoftDelete();

            await _userManager.UpdateAsync(user);

            return ApiResponse<object>.ReturnSuccessResponse(Messages.UserDeleted, Messages.UserDeleted);
        }
        public async Task<ApiResponse<GetUserProfileResponseDto>> GetCurrentUserProfileAsync()
        {
            if (!_userContext.IsAuthenticated || _userContext.UserId == null)
            {
                return ApiResponse<GetUserProfileResponseDto>
                    .ReturnFailureResponse(Messages.InvalidToken, HttpStatusCode.Unauthorized);
            }

            var user = await _userManager.FindByIdAsync(_userContext.UserId.ToString());

            if (user == null)
            {
                return ApiResponse<GetUserProfileResponseDto>.ReturnFailureResponse(Messages.UserNotFound, HttpStatusCode.NotFound);
            }

            var result = new GetUserProfileResponseDto
            {
                Id = user.Id,
                DisplayName = user.DisplayName,
                Email = user.Email!,
                PhoneNumber = user.PhoneNumber
            };

            return ApiResponse<GetUserProfileResponseDto>.ReturnSuccessResponse(result);
        }

        public async Task<ApiResponse<object>> EditUserProfileAsync(EditProfileRequestDto editProfileRequestDto)
        {
            if(!_userContext.IsAuthenticated  || _userContext.UserId == null)
            {
                return ApiResponse<object>.ReturnFailureResponse(Messages.InvalidToken, HttpStatusCode.Unauthorized);
            }

            var user = await _userManager.FindByIdAsync(_userContext.UserId.ToString());

            if (user == null)
            {
                return ApiResponse<object>.ReturnFailureResponse(Messages.UserNotFound, HttpStatusCode.NotFound);
            }

            user.DisplayName = editProfileRequestDto.DisplayName;
            user.PhoneNumber = editProfileRequestDto.PhoneNumber;

            if (!string.Equals(user.Email, editProfileRequestDto.Email, StringComparison.OrdinalIgnoreCase))
            {
                var emailExists = await _userManager.FindByEmailAsync(editProfileRequestDto.Email);
                if (emailExists != null)
                {
                    return ApiResponse<object>.ReturnFailureResponse(Messages.EmailExists, HttpStatusCode.BadRequest);
                }

                user.Email = editProfileRequestDto.Email;
            }

            if (!string.IsNullOrWhiteSpace(editProfileRequestDto.NewPassword))
            {
                if (editProfileRequestDto.NewPassword != editProfileRequestDto.ConfirmNewPassword)
                {
                    return ApiResponse<object>.ReturnFailureResponse(Messages.PasswordsDoNotMatch, HttpStatusCode.BadRequest);
                }

                var passwordResult = await _userManager.ChangePasswordAsync(user, editProfileRequestDto.CurrentPassword!, editProfileRequestDto.NewPassword);

                if (!passwordResult.Succeeded)
                {
                    return ApiResponse<object>.ReturnFailureResponse(Messages.CurrentPasswordIncorrect, HttpStatusCode.BadRequest);
                }
            }
            await _userManager.UpdateAsync(user);

            return ApiResponse<object>.ReturnSuccessResponse(Messages.ProfileUpdated, Messages.ProfileUpdated);
        }



    }
}
