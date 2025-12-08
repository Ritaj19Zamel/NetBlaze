using NetBlaze.SharedKernel.SharedResources;
using System.ComponentModel.DataAnnotations;

namespace NetBlaze.SharedKernel.Dtos.Vacation.Requests
{
    public sealed record CreateVacationRequestDto
    {

        public DayOfWeek? DayName { get; set; }

        public DateOnly? DayDate { get; set; }
        [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = nameof(Messages.FieldRequired))]

        public bool IsVacation { get; set; }
        [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = nameof(Messages.FieldRequired))]

        public bool IsRecurring { get; set; }
    }
}
