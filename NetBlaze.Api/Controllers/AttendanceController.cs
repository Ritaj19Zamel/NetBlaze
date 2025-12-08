
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetBlaze.Application.Interfaces.ServicesInterfaces;
using NetBlaze.SharedKernel.HelperUtilities.General;

namespace NetBlaze.Api.Controllers
{
    [Authorize]
    public class AttendanceController : BaseNetBlazeController, IAttendanceService
    {
        
        private readonly IAttendanceService _attendanceService;

        public AttendanceController(IAttendanceService attendanceService)
        {
            _attendanceService = attendanceService;
        }
        [HttpPost("attend")]
        public async Task<ApiResponse<string>> AddAttendanceAsync(CancellationToken cancellationToken)
        {
            return await _attendanceService.AddAttendanceAsync(cancellationToken);
        }
    }
}
