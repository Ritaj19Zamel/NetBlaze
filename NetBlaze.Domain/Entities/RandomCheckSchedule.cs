using NetBlaze.Domain.Common;

namespace NetBlaze.Domain.Entities
{
    public class RandomCheckSchedule : BaseEntity<long>
    {
        public DateTime ScheduledAt { get; set; }
        public bool IsSent { get; set; }
        public DateTime? SentAt { get; set; }
    }
}
