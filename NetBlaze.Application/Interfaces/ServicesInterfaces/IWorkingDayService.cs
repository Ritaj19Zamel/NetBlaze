

namespace NetBlaze.Application.Interfaces.ServicesInterfaces
{
    public interface IWorkingDayService
    {
        Task<object> IsVacationDayAsync(DateOnly todayDate, CancellationToken cancellationToken = default);
    }
}
