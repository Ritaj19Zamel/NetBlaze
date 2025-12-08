using NetBlaze.Domain.Common;
using NetBlaze.Domain.Entities.Identity;

namespace NetBlaze.Domain.Entities
{
    public class EmployeeAttendence : BaseEntity<long>
    {
        public long UserId { get; set; }
        public DateOnly AttendDate { get; set; }
        public TimeOnly AttendTime { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<AttendencePolicyAction> AttendencePolicyActions { get; set; } = [];

    }
}
