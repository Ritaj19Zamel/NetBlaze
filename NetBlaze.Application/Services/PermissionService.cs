

using NetBlaze.Application.Interfaces.General;
using NetBlaze.Application.Interfaces.ServicesInterfaces;
using NetBlaze.Domain.Entities;
using System.Security.Claims;

namespace NetBlaze.Application.Services
{
    public class PermissionService : IPermissionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserContext _userContext;

        public PermissionService(IUnitOfWork unitOfWork, IUserContext userContext)
        {
            _unitOfWork = unitOfWork;
            _userContext = userContext;
        }

        public async Task<bool> UserHasPermissionAsync(string path, string method)
        {
            if (!_userContext.IsAuthenticated)
                return false;

            var roles = _userContext.Roles; 

            path = path.ToLower();
            method = method.ToUpper();

            var permission = await _unitOfWork.Repository
                .GetSingleAsync<Permission>(
                    true,
                    p => p.Path.ToLower() == path && p.HttpMethod == method
                );

            if (permission == null)
                return false;

            return await _unitOfWork.Repository.AnyAsync<RolePermission>(
                rp => rp.PermissionId == permission.Id &&
                      roles.Contains(rp.Role.Name)
            );
        }
    }
}
