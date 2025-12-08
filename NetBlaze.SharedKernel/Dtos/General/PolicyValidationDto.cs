using NetBlaze.SharedKernel.Enums;
namespace NetBlaze.SharedKernel.Dtos.General
{
    public sealed record PolicyValidationDto
    {
        public long PolicyId { get; set; }
        public string PolicyCode { get; set; }
        public PolicyType PolicyType { get; set; }
        public TimeOnly WorkStartTime { get; set; }
        public TimeOnly WorkEndTime { get; set; }
        public int RequiredHours { get; set; }
        public long? IgnoreId { get; set; }
    }
}
