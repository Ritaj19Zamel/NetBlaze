using NetBlaze.Domain.Common;

namespace NetBlaze.Domain.Entities
{
    public class Vacation : BaseEntity <long>
    {
        public DayOfWeek? DayName { get; set; }
        public DateOnly? DayDate {  get; set; }
        public bool IsVacation { get; set; }
        public bool IsRecurring { get; set; }

    }
}
