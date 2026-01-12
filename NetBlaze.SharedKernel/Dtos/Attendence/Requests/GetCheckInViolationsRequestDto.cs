
using NetBlaze.SharedKernel.Dtos.General;
using NetBlaze.SharedKernel.Enums;
using NetBlaze.SharedKernel.SharedResources;
using System.ComponentModel.DataAnnotations;

namespace NetBlaze.SharedKernel.Dtos.Attendence.Requests
{
    public sealed record GetCheckInViolationsRequestDto
    {
        [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = nameof(Messages.FieldRequired))]
        public DateOnly FromDate { get; set; }

        [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = nameof(Messages.FieldRequired))]
        public DateOnly ToDate { get; set; }

        public ViolationStatus? ViolationStatus { get; set; }

        [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = nameof(Messages.FieldRequired))]
        public PaginationRequestDto Pagination { get; set;  } = new PaginationRequestDto();

    }
}
