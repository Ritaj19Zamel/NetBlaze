using Microsoft.AspNetCore.Mvc;
using NetBlaze.Application.Interfaces.ServicesInterfaces;
using NetBlaze.SharedKernel.Dtos.Role.Responses;
using NetBlaze.SharedKernel.HelperUtilities.General;

namespace NetBlaze.Api.Controllers
{
    public class RoleController : BaseNetBlazeController, IRoleService
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet]
        public async Task<ApiResponse<List<GetRoleResponseDto>>> GetAllAsync(
            CancellationToken cancellationToken = default)
        {
            return await _roleService.GetAllAsync(cancellationToken);
        }

        [HttpGet("{id}")]
        public async Task<ApiResponse<GetRoleResponseDto>> GetByIdAsync(
            long id,
            CancellationToken cancellationToken = default)
        {
            return await _roleService.GetByIdAsync(id, cancellationToken);
        }
    }
}
