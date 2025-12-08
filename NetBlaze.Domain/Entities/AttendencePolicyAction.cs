

using NetBlaze.Domain.Common;

namespace NetBlaze.Domain.Entities
{
    public class AttendencePolicyAction : BaseEntity<long>
    {
        public long AttendenceId { get; set; }
        public long PolicyId { get; set; }
        public bool IsApplied { get; set; }
        public string Clarification { get; set; }
        public virtual EmployeeAttendence EmployeeAttendence { get; set; }
        public virtual Policy Policy { get; set; }
    }
}
