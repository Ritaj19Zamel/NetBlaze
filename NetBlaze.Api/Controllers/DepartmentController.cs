using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetBlaze.Application.Interfaces.ServicesInterfaces;
using NetBlaze.SharedKernel.Dtos.Department.Requests;
using NetBlaze.SharedKernel.Dtos.Department.Responses;
using NetBlaze.SharedKernel.HelperUtilities.General;

namespace NetBlaze.Api.Controllers
{
    [Authorize(Policy = AuthorizationPolicies.HRAndManager)]
    public class DepartmentController : BaseNetBlazeController, IDepartmentService
    {
        private readonly IDepartmentService _departmentService;

        public DepartmentController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        [HttpPost("CreateDepartment")]
        public async Task<ApiResponse<object>> CreateDepartmentAsync(CreateDepartmentRequestDto dto, CancellationToken cancellationToken = default)
        {
            return await _departmentService.CreateDepartmentAsync(dto, cancellationToken);
        }

        [HttpDelete("DeleteDepartment/{id:long}")]
        public async Task<ApiResponse<object>> DeleteDepartmentAsync(long id, CancellationToken cancellationToken = default)
        {
            return await _departmentService.DeleteDepartmentAsync(id, cancellationToken);
        }

        [HttpGet("GetAllDepartments")]
        public async Task<ApiResponse<List<GetDepartmentResponseDto>>> GetAllDepartmentAsync(CancellationToken cancellationToken = default)
        {
            return await _departmentService.GetAllDepartmentAsync(cancellationToken);
        }

        [HttpGet("GetDepartmentById/{id:long}")]
        public async Task<ApiResponse<GetDepartmentResponseDto>> GetDepartmentByIdAsync(long id, CancellationToken cancellationToken = default)
        {
            return await _departmentService.GetDepartmentByIdAsync(id, cancellationToken);
        }

        [HttpPost("ToggleDepartmentStatus/{id:long}")]
        public async Task<ApiResponse<object>> ToggleDepartmentStatusAsync(long id, CancellationToken cancellationToken = default)
        {
            return await _departmentService.ToggleDepartmentStatusAsync(id, cancellationToken);
        }

        [HttpPut("UpdateDepartment")]
        public async Task<ApiResponse<object>> UpdateDepartmentAsync(UpdateDepartmentRequestDto dto, CancellationToken cancellationToken = default)
        {
            return await _departmentService.UpdateDepartmentAsync(dto, cancellationToken);
        }
    }
}
