
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetBlaze.Application.Interfaces.ServicesInterfaces;
using NetBlaze.Application.Mappings;
using NetBlaze.SharedKernel.Dtos.Attendence.Requests;
using NetBlaze.SharedKernel.Dtos.Attendence.Responses;
using NetBlaze.SharedKernel.HelperUtilities.General;

namespace NetBlaze.Api.Controllers
{
    public class AttendanceController : BaseNetBlazeController, IAttendanceService
    {
        
        private readonly IAttendanceService _attendanceService;

        public AttendanceController(IAttendanceService attendanceService)
        {
            _attendanceService = attendanceService;
        }

        [Authorize(Policy = AuthorizationPolicies.Employee)]
        [HttpPost("Attend")]
        public async Task<ApiResponse<object>> AddAttendanceAsync(CancellationToken cancellationToken)
        {
            return await _attendanceService.AddAttendanceAsync(cancellationToken);
        }

        [Authorize(Policy = AuthorizationPolicies.HRAndManager)]
        [HttpPost("ApprovePolicy")]
        public async Task<ApiResponse<object>> ApprovePolicyRequestAsync(ApprovePolicyRequestDto approvePolicyRequestDto, CancellationToken cancellationToken)
        {
            return await _attendanceService.ApprovePolicyRequestAsync(approvePolicyRequestDto, cancellationToken);
        }


        [Authorize(Policy = AuthorizationPolicies.HRAndManager)]
        [HttpGet("GetAttendanceReport")]
        public async Task<ApiResponse<PaginatedList<GetAttendanceResponseDto>>> GetAttendanceReportAsync([FromQuery]GetAttendanceRequestDto getAttendanceRequestDto, CancellationToken cancellationToken = default)
        {
            return await _attendanceService.GetAttendanceReportAsync(getAttendanceRequestDto, cancellationToken);
        }

        [Authorize(Policy = AuthorizationPolicies.HRAndManager)]
        [HttpGet("GetCheckViolations")]
        public async Task<ApiResponse<PaginatedList<GetCheckInViolationResponseDto>>> GetCheckInViolations([FromQuery]GetCheckInViolationsRequestDto getCheckInViolationsRequestDto
            , CancellationToken cancellationToken = default)
        {
            return await _attendanceService.GetCheckInViolations(getCheckInViolationsRequestDto, cancellationToken);
        }

        [Authorize(Policy = AuthorizationPolicies.Employee)]
        [HttpGet("GetTodayAttendance")]
        public async Task<ApiResponse<GetTodayAttendanceResponseDto>> GetTodayAttendanceAsync(CancellationToken cancellationToken)
        {
            return await _attendanceService.GetTodayAttendanceAsync(cancellationToken);
        }
    }
}
