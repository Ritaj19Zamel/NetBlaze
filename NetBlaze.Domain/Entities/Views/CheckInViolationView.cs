using NetBlaze.SharedKernel.Enums;

namespace NetBlaze.Domain.Entities.Views
{
    public class CheckInViolationView
    {
        public long AttendanceId { get; set; }
        public long UserId { get; set; }
        public string UserName { get; set; }
        public DateOnly AttendDate { get; set; }
        public long PolicyId { get; set; }
        public string PolicyName { get; set; }
        public PolicyType PolicyType { get; set; }
        public string PolicyCode { get; set; }
        public string Clarification { get; set; }
        public double ViolationValue { get; set; }
        public bool? IsApplied { get; set; }
        public ViolationStatus  ViolationStatus { get; set; }
    }
}
