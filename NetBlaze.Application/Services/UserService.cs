using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NetBlaze.Application.Interfaces.General;
using NetBlaze.Application.Interfaces.ServicesInterfaces;
using NetBlaze.Domain.Entities;
using NetBlaze.Domain.Entities.Identity;
using NetBlaze.SharedKernel.Dtos;
using NetBlaze.SharedKernel.Dtos.User.Requests;
using NetBlaze.SharedKernel.Dtos.User.Responses;
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
            if (managers == null)
                return ApiResponse<List<GetManagerResponseDto>>.ReturnFailureResponse(Messages.NoManagers, HttpStatusCode.NotFound);
            return ApiResponse<List<GetManagerResponseDto>>.ReturnSuccessResponse(managers);

        }
        public async Task<ApiResponse<object>> UpdateUserAsync(UpdateUserRequestDto updateUserRequestDto, CancellationToken cancellationToken = default)
        {
            if (!_userContext.IsAuthenticated || _userContext.UserId == 0)
            {
                return ApiResponse<object>.ReturnFailureResponse(Messages.InvalidToken, HttpStatusCode.Unauthorized);
            }

            var user = await _userManager.Users
                .Include(u => u.UserDetail)
                .Include(u => u.UserRoles)
                .FirstOrDefaultAsync(u => u.Id == _userContext.UserId, cancellationToken);

            if (user == null)
            {
                return ApiResponse<object>.ReturnFailureResponse(Messages.UserNotFound, HttpStatusCode.NotFound);
            }

            user.DisplayName = updateUserRequestDto.DisplayName;
            user.PhoneNumber = updateUserRequestDto.PhoneNumber;
            user.DepartmentId = updateUserRequestDto.DepartmentId;
            user.ManagerId = updateUserRequestDto.ManagerId == 0 ? null : updateUserRequestDto.ManagerId;

            if (user.UserDetail == null)
                user.UserDetail = new UserDetail { UserId = user.Id };
            user.UserDetail.DeviceName = updateUserRequestDto.DeviceName;
            user.UserDetail.CertificatePassword = updateUserRequestDto.CertificatePassword;

            var currentRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, currentRoles);

            var newRole = await _roleManager.FindByIdAsync(updateUserRequestDto.RoleId.ToString());
            await _userManager.AddToRoleAsync(user, newRole.Name);

            await _userManager.UpdateAsync(user);

            return ApiResponse<object>.ReturnSuccessResponse(Messages.UserUpdated, Messages.UserUpdated);

        }
        public async Task<ApiResponse<object>> AddUserAsync(
            AddUserRequestDto dto,
            CancellationToken cancellationToken = default)
        {
            var existingUser = await _userManager.FindByEmailAsync(dto.Email);
            if (existingUser != null)
                return ApiResponse<object>.ReturnFailureResponse(
                    "UserAlreadyExists",
                    HttpStatusCode.BadRequest);

            if (dto.ManagerId.HasValue && dto.ManagerId != 0)
            {
                var managerExists = await _userManager.Users
                    .AnyAsync(u => u.Id == dto.ManagerId.Value, cancellationToken);

                if (!managerExists)
                    return ApiResponse<object>.ReturnFailureResponse(
                        "ManagerNotExists",
                        HttpStatusCode.BadRequest);
            }

            if (dto.DepartmentId != 0)
            {
                var departmentExists = await _unitOfWork.Repository
                    .AnyAsync<Department>(d => d.Id == dto.DepartmentId, cancellationToken);

                if (!departmentExists)
                    return ApiResponse<object>.ReturnFailureResponse(
                        "DepartmentNotExists",
                        HttpStatusCode.BadRequest);
            }

            var role = await _roleManager.FindByIdAsync(dto.RoleId.ToString());
            if (role == null)
                return ApiResponse<object>.ReturnFailureResponse(
                    Messages.RoleNotExists,
                    HttpStatusCode.BadRequest);

            var user = new User
            {
                UserName = dto.Email,
                Email = dto.Email,
                DisplayName = dto.DisplayName,
                PhoneNumber = dto.PhoneNumber,
                DepartmentId = dto.DepartmentId,
                ManagerId = dto.ManagerId == 0 ? null : dto.ManagerId,
                EmailConfirmed = true
            };

            var createResult = await _userManager.CreateAsync(user, dto.Password);

            if (!createResult.Succeeded)
                return ApiResponse<object>.ReturnFailureResponse(
                    createResult.Errors.First().Description,
                    HttpStatusCode.BadRequest);

            await _userManager.AddToRoleAsync(user, role.Name!);

            return ApiResponse<object>.ReturnSuccessResponse("UserCreated");
        }
    }
}
