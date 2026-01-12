

using NetBlaze.Application.Interfaces.General;
using NetBlaze.Application.Interfaces.ServicesInterfaces;
using NetBlaze.Domain.Entities;

namespace NetBlaze.Application.Services
{
    public class WorkingDayService : IWorkingDayService
    {
        private readonly IUnitOfWork _unitOfWork;
        public WorkingDayService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<object> IsVacationDayAsync(DateOnly todayDate, CancellationToken cancellationToken = default)
        {
            var todayName = DateTime.Now.DayOfWeek;

            var vacation = await _unitOfWork.Repository.GetSingleAsync<Vacation>(
                            asNoTracking: true,
                            v => (v.IsRecurring &&
                                 v.DayName != null &&
                                 v.DayName == todayName) ||
                                (v.IsRecurring &&
                                 v.DayDate != null &&
                                 v.DayDate.Value.Day == todayDate.Day &&
                                 v.DayDate.Value.Month == todayDate.Month)  ||
                                (!v.IsRecurring &&
                                 v.DayDate != null &&
                                 v.DayDate == todayDate),
                            cancellationToken
                        );

            if (vacation != null)
            {
                return true;
            }

            return false;
        }
    }
}
