

namespace NetBlaze.SharedKernel.Dtos.Attendence.Responses
{
    public sealed record GetTodayAttendanceResponseDto
    {
        public DateOnly Date { get; set; }
        public TimeOnly? CheckIn { get; set; }
        public TimeOnly? CheckOut { get; set; }
    }
}
