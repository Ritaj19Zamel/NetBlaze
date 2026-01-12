using NetBlaze.SharedKernel.HelperUtilities.General;
using NetBlaze.SharedKernel.SharedResources;
using System.ComponentModel.DataAnnotations;

namespace NetBlaze.SharedKernel.Dtos.Vacation.Requests
{
    public sealed record UpdateVacationRequestDto
    {
        [IgnoreReflectionMapping]
        public long Id { get; set; }
        public DayOfWeek? DayName { get; set; }
        public DateOnly? DayDate { get; set; }
        [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = nameof(Messages.FieldRequired))]

        public bool IsVacation { get; set; }
        [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = nameof(Messages.FieldRequired))]

        public bool IsRecurring { get; set; }
    }
}
