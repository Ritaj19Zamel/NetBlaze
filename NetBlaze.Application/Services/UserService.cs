using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NetBlaze.Application.Interfaces.General;
using NetBlaze.Application.Interfaces.ServicesInterfaces;
using NetBlaze.Domain.Entities;
using NetBlaze.Domain.Entities.Identity;
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
            if(managers == null)
                return ApiResponse<List<GetManagerResponseDto>>.ReturnFailureResponse(Messages.NoManagers, HttpStatusCode.NotFound);
            return ApiResponse<List<GetManagerResponseDto>>.ReturnSuccessResponse(managers);

        }

        public async Task<ApiResponse<string>> UpdateUserAsync(UpdateUserRequestDto dto, CancellationToken cancellationToken = default)
        {
            if (!_userContext.IsAuthenticated || _userContext.UserId == 0)
                return ApiResponse<string>.ReturnFailureResponse(Messages.InvalidToken, HttpStatusCode.Unauthorized);
            var user = await _userManager.Users
                .Include(u => u.UserDetail)
                .Include(u => u.UserRoles)
                .FirstOrDefaultAsync(u => u.Id == _userContext.UserId, cancellationToken);
            if(user == null)
                return ApiResponse<string>.ReturnFailureResponse(Messages.UserNotFound, HttpStatusCode.NotFound);
            user.DisplayName = dto.DisplayName;
            user.PhoneNumber = dto.PhoneNumber;
            user.DepartmentId = dto.DepartmentId;
            user.ManagerId = dto.ManagerId == 0 ? null : dto.ManagerId;
            if (user.UserDetail == null)
                user.UserDetail = new UserDetail { UserId = user.Id };
            user.UserDetail.DeviceName = dto.DeviceName;
            user.UserDetail.CertificatePassword = dto.CertificatePassword;
            var currentRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, currentRoles);

            var newRole = await _roleManager.FindByIdAsync(dto.RoleId.ToString());
            await _userManager.AddToRoleAsync(user, newRole.Name);

            await _userManager.UpdateAsync(user);

            return ApiResponse<string>.ReturnSuccessResponse(Messages.UserUpdated, Messages.UserUpdated);

        }
    }
}
