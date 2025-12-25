namespace NetBlaze.SharedKernel.Dtos.Attendence.Responses
{
    public sealed record GetAttendanceResponseDto
    {
        public long UserId { get; set; }
        public string DisplayName { get; set; }

        public DateOnly Date { get; set; }
        public TimeOnly? CheckIn { get; set; }
        public TimeOnly? CheckOut { get; set; }
    }
}
