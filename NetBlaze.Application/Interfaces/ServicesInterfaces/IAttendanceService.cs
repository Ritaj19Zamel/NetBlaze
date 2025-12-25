

using NetBlaze.Application.Mappings;
using NetBlaze.SharedKernel.Dtos.Attendence.Requests;
using NetBlaze.SharedKernel.Dtos.Attendence.Responses;
using NetBlaze.SharedKernel.HelperUtilities.General;

namespace NetBlaze.Application.Interfaces.ServicesInterfaces
{
    public interface IAttendanceService
    {
        Task<ApiResponse<object>> AddAttendanceAsync(CancellationToken cancellationToken = default);
        Task<ApiResponse<PaginatedList<GetAttendanceResponseDto>>> GetAttendanceReportAsync(GetAttendanceRequestDto getAttendanceRequestDto,
            CancellationToken cancellationToken = default);
        Task<ApiResponse<PaginatedList<GetCheckInViolationResponseDto>>> GetCheckInViolations(GetCheckInViolationsRequestDto getCheckInViolationsRequestDto
            , CancellationToken cancellationToken = default);
        Task<object> ApprovePolicyRequestAsync(ApprovePolicyRequestDto approvePolicyRequestDto,
             CancellationToken cancellationToken);
    }
}
