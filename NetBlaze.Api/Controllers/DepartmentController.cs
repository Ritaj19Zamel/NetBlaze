using Microsoft.AspNetCore.Mvc;
using NetBlaze.Application.Interfaces.ServicesInterfaces;
using NetBlaze.SharedKernel.Dtos.Department.Responses;
using NetBlaze.SharedKernel.HelperUtilities.General;

namespace NetBlaze.Api.Controllers
{
    [Route("api/department")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentService _departmentService;

        public DepartmentController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        [HttpGet("all")]
        public async Task<ApiResponse<List<GetDepartmentResponseDto>>> GetAllAsync(
            CancellationToken cancellationToken = default)
        {
            return await _departmentService.GetAllAsync(cancellationToken);
        }

        [HttpGet("{id:long}")]
        public async Task<ApiResponse<GetDepartmentResponseDto>> GetByIdAsync(
            long id,
            CancellationToken cancellationToken = default)
        {
            return await _departmentService.GetByIdAsync(id, cancellationToken);
        }
    }
}
