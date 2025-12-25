

using Microsoft.AspNetCore.Mvc;
using NetBlaze.Application.Interfaces.ServicesInterfaces;
using NetBlaze.SharedKernel.Dtos.Department.Responses;
using NetBlaze.SharedKernel.HelperUtilities.General;

namespace NetBlaze.Api.Controllers
{

    public class DepartmentController : BaseNetBlazeController, IDepartmentService
    {
        private readonly IDepartmentService _departmentService;

        public DepartmentController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        [HttpGet("GetAllDepartments")]
        public async Task<ApiResponse<List<GetDepartmentResponseDto>>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _departmentService.GetAllAsync(cancellationToken);
        }

        [HttpGet("GetDepartmentById{id:long}")]
        public async Task<ApiResponse<GetDepartmentResponseDto>> GetByIdAsync(long id, CancellationToken cancellationToken)
        {
            return await _departmentService.GetByIdAsync(id, cancellationToken);
        }
    }
}
