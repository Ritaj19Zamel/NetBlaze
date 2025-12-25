
using NetBlaze.Domain.Common;
using NetBlaze.SharedKernel.Enums;

namespace NetBlaze.Domain.Entities
{
    public class Policy : BaseEntity<long>
    {
        public string PolicyName { get; set; }
        public string PolicyCode { get; set; }
        public TimeOnly? WorkStartTime { get; set; }
        public TimeOnly? WorkEndTime { get; set; }
        public PolicyType PolicyType { get; set; }
        public double? ActionValue { get; set; }
        public int? RequiredHours { get; set; }
        public virtual ICollection<AttendencePolicyAction> AttendencePolicyActions { get; set; } = [];

    }
}
