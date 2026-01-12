

using NetBlaze.Domain.Common;

namespace NetBlaze.Domain.Entities
{
    public class RandomCheckAutoConfig : BaseEntity<long>
    {
        public bool IsEnabled { get; set; }

        public DateOnly FromDate { get; set; }
        public DateOnly ToDate { get; set; }

        public TimeSpan FromTime { get; set; }
        public TimeSpan ToTime { get; set; }

        public int ChecksPerDay { get; set; }

        public bool SendToAllEmployees { get; set; }

    }
}

