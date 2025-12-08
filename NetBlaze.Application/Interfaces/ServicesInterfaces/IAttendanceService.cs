

using NetBlaze.SharedKernel.HelperUtilities.General;

namespace NetBlaze.Application.Interfaces.ServicesInterfaces
{
    public interface IAttendanceService
    {
        Task<ApiResponse<string>> AddAttendanceAsync(CancellationToken cancellationToken = default);
    }
}
