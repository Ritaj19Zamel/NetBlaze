
using NetBlaze.SharedKernel.Enums;

namespace NetBlaze.SharedKernel.Dtos.Attendence.Responses
{
    public sealed record GetCheckInViolationResponseDto
    {
        public long UserId { get; set; }
        public string UserName { get; set; }
        public long PolicyId { get; set; }
        public string PolicyName { get; set; }
        public string PolicyCode { get; set; }
        public DateOnly AttendDate { get; set; }
        public ViolationStatus ViolationStatus { get; set; }
        public bool? IsApplied { get; set; }
        public string Clarification { get; set; }
        public int ViolationsCount { get; set; }
        public double TotalViolationValue { get; set; }
    }
}
