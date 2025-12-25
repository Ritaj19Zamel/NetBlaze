
namespace NetBlaze.SharedKernel.Dtos.Attendence.Responses
{
    public sealed record GetCheckInViolationResponseDto
    {
        public long AttendanceId { get; set; }
        public long UserId { get; set; }
        public string UserName { get; set; }

        public long PolicyId { get; set; }
        public string PolicyName { get; set; }
        public string PolicyCode { get; set; }

        public DateOnly AttendDate { get; set; }

        public string Clarification { get; set; }
        public double ViolationValue { get; set; }
    }
}
