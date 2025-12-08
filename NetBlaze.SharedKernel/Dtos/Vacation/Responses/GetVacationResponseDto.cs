namespace NetBlaze.SharedKernel.Dtos.Vacation.Responses
{
    public sealed record GetVacationResponseDto
    {
        public long Id { get; set; }
        public string DayName { get; set; }
        public DateOnly? DayDate { get; set; }
        public bool IsVacation { get; set; }
        public bool IsRecurring { get; set; }
    }
}
