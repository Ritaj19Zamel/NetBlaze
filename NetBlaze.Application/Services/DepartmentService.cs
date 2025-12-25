using NetBlaze.Application.Interfaces.General;
using NetBlaze.Application.Interfaces.ServicesInterfaces;
using NetBlaze.Domain.Entities;
using NetBlaze.SharedKernel.Dtos.Department.Responses;
using NetBlaze.SharedKernel.HelperUtilities.General;
using NetBlaze.SharedKernel.SharedResources;
using System.Net;

namespace NetBlaze.Application.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IUnitOfWork _unitOfWork;
        public DepartmentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ApiResponse<List<GetDepartmentResponseDto>>> GetAllAsync(
            CancellationToken cancellationToken = default)
        {
            var departments = await _unitOfWork.Repository
                .GetMultipleAsync<Department, GetDepartmentResponseDto>(
                    true,
                    e => new GetDepartmentResponseDto
                    {
                        Id = e.Id,
                        Name = e.DepartmentName
                    },
                    cancellationToken);

            return ApiResponse<List<GetDepartmentResponseDto>>
                .ReturnSuccessResponse(departments ?? new List<GetDepartmentResponseDto>());
        }

        public async Task<ApiResponse<GetDepartmentResponseDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default)
        {
            var department = await _unitOfWork.Repository
                .GetByIdAsync<Department, GetDepartmentResponseDto>(true,
                             id,
                             e => new GetDepartmentResponseDto
                             {
                                 Id = e.Id,
                                 Name = e.DepartmentName
                             },cancellationToken);
            if(department == null)
                return ApiResponse<GetDepartmentResponseDto>.ReturnFailureResponse(Messages.DepartmentNotExists, HttpStatusCode.NotFound);
            return ApiResponse<GetDepartmentResponseDto>.ReturnSuccessResponse(department);
        }
    }
}
