using Microsoft.EntityFrameworkCore;
using NetBlaze.Application.Interfaces.General;
using NetBlaze.Application.Interfaces.ServicesInterfaces;
using NetBlaze.Domain.Entities;
using NetBlaze.Domain.Entities.Identity;
using NetBlaze.SharedKernel.Dtos.Role.Responses;
using NetBlaze.SharedKernel.HelperUtilities.General;
using NetBlaze.SharedKernel.SharedResources;
using System.Net;

namespace NetBlaze.Application.Services
{
    public class RoleService : IRoleService
    {
        private readonly IUnitOfWork _unitOfWork;
        public RoleService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResponse<List<GetRoleResponseDto>>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var roles = await _unitOfWork.Repository.GetMultipleAsync<Role, GetRoleResponseDto>(true,
                e => new GetRoleResponseDto()
                {
                    Id = e.Id,
                    RoleName = e.Name!
                },cancellationToken);
            if (roles == null)
                return ApiResponse<List<GetRoleResponseDto>>.ReturnFailureResponse(Messages.NoRoles, HttpStatusCode.NotFound);

            return ApiResponse<List<GetRoleResponseDto>>.ReturnSuccessResponse(roles);
        }

        public async Task<ApiResponse<GetRoleResponseDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default)
        {
            var role = await _unitOfWork.Repository.GetByIdAsync<Role, GetRoleResponseDto>(true, id,
                e => new GetRoleResponseDto()
                {
                    Id = e.Id,
                    RoleName = e.Name!
                }, cancellationToken);
            if(role == null)
                return ApiResponse<GetRoleResponseDto>.ReturnFailureResponse(Messages.RoleNotExists, HttpStatusCode.NotFound);
            return ApiResponse<GetRoleResponseDto>.ReturnSuccessResponse(role);


        }
    }
}
