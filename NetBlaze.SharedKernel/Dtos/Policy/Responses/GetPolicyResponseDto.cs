

using NetBlaze.SharedKernel.Enums;

namespace NetBlaze.SharedKernel.Dtos.Policy.Responses
{
    public sealed record GetPolicyResponseDto
    {
        public long Id { get; set; }
        public string PolicyName { get; set; }
        public string PolicyCode { get; set; }
        public TimeOnly WorkStartTime { get; set; }
        public TimeOnly WorkEndTime { get; set; }
        public PolicyType PolicyType { get; set; }
        public double ActionValue { get; set; }
        public int RequiredHours { get; set; }
    }
}
