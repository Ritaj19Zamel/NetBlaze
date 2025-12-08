
namespace NetBlaze.SharedKernel.Dtos.General
{
    public sealed record VacationValidationDto
    {
        public long? VacationId { get; set; }
        public DayOfWeek? DayName { get; set; }
        public DateOnly? DayDate { get; set; }
        public bool IsRecurring { get; set; }
    }
}
