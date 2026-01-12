

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetBlaze.Application.Interfaces.ServicesInterfaces;
using NetBlaze.SharedKernel.Dtos.Role.Responses;
using NetBlaze.SharedKernel.HelperUtilities.General;

namespace NetBlaze.Api.Controllers
{
    [Authorize(Policy = AuthorizationPolicies.HRAndManager)]
    public class RoleController : BaseNetBlazeController, IRoleService
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet("GetAllRoles")]
        public async Task<ApiResponse<List<GetRoleResponseDto>>> GetAllRolesAsync(CancellationToken cancellationToken = default)
        {
            return await _roleService.GetAllRolesAsync(cancellationToken);
        }

        [HttpGet("GetRoleById/{id:long}")]
        public async Task<ApiResponse<GetRoleResponseDto>> GetRolesByIdAsync(long id, CancellationToken cancellationToken = default)
        {
            return await _roleService.GetRolesByIdAsync(id, cancellationToken);
        }
    }
}
