

namespace NetBlaze.SharedKernel.Dtos.RandomCheck.Requests
{
    public sealed record AutoRandomCheckRequestDto
    {
        public bool IsEnabled { get; set; }

        public DateOnly FromDate { get; set; }
        public DateOnly ToDate { get; set; }

        public TimeSpan FromTime { get; set; }
        public TimeSpan ToTime { get; set; }

        public int ChecksPerDay { get; set; }
    }
}
