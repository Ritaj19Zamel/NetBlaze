

using NetBlaze.SharedKernel.SharedResources;
using System.ComponentModel.DataAnnotations;

namespace NetBlaze.SharedKernel.Dtos.Attendence.Requests
{
    public record ApprovePolicyRequestDto
    {
        [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = nameof(Messages.FieldRequired))]
        public long UserId { get; set; }

        [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = nameof(Messages.FieldRequired))]
        public long PolicyId { get; set; }

        [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = nameof(Messages.FieldRequired))]
        public DateOnly FromDate { get; set; }

        [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = nameof(Messages.FieldRequired))]
        public DateOnly ToDate { get; set; }

        [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = nameof(Messages.FieldRequired))]
        public bool IsApplied { get; set; }
        public string? Clarification { get; set; }
    }
}
