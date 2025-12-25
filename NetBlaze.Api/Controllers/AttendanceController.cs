
using Microsoft.AspNetCore.Mvc;
using NetBlaze.Api.Filters;
using NetBlaze.Application.Interfaces.ServicesInterfaces;
using NetBlaze.Application.Mappings;
using NetBlaze.SharedKernel.Dtos.Attendence.Requests;
using NetBlaze.SharedKernel.Dtos.Attendence.Responses;
using NetBlaze.SharedKernel.HelperUtilities.General;

namespace NetBlaze.Api.Controllers
{
    [DynamicAuthorize]
    public class AttendanceController : BaseNetBlazeController, IAttendanceService
    {
        
        private readonly IAttendanceService _attendanceService;

        public AttendanceController(IAttendanceService attendanceService)
        {
            _attendanceService = attendanceService;
        }

        [HttpPost("attend")]
        public async Task<ApiResponse<object>> AddAttendanceAsync(CancellationToken cancellationToken)
        {
            return await _attendanceService.AddAttendanceAsync(cancellationToken);
        }

        [HttpPost("approve-policy")]
        public async Task<object> ApprovePolicyRequestAsync(ApprovePolicyRequestDto approvePolicyRequestDto, CancellationToken cancellationToken)
        {
            return await _attendanceService.ApprovePolicyRequestAsync(approvePolicyRequestDto, cancellationToken);
        }

        [HttpGet("get-attendance-report")]
        public async Task<ApiResponse<PaginatedList<GetAttendanceResponseDto>>> GetAttendanceReportAsync([FromQuery]GetAttendanceRequestDto getAttendanceRequestDto, CancellationToken cancellationToken = default)
        {
            return await _attendanceService.GetAttendanceReportAsync(getAttendanceRequestDto, cancellationToken);
        }

        [HttpGet("get-check-violations")]
        public async Task<ApiResponse<PaginatedList<GetCheckInViolationResponseDto>>> GetCheckInViolations([FromQuery]GetCheckInViolationsRequestDto getCheckInViolationsRequestDto
            , CancellationToken cancellationToken = default)
        {
            return await _attendanceService.GetCheckInViolations(getCheckInViolationsRequestDto, cancellationToken);
        }
    }
}
