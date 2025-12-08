using NetBlaze.SharedKernel.HelperUtilities.Constants;
using NetBlaze.SharedKernel.SharedResources;
using System.ComponentModel.DataAnnotations;

namespace NetBlaze.SharedKernel.Dtos.Auth.Requests
{
    public sealed record RegisterRequestDto
    {
        [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = nameof(Messages.FieldRequired))]
        [RegularExpression(RegexTemplate.Email,ErrorMessageResourceType = typeof(Messages),ErrorMessageResourceName = nameof(Messages.InvalidEmailFormat))]
        public string Email { get; set; }
        [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = nameof(Messages.FieldRequired))]
        [RegularExpression(RegexTemplate.Password,ErrorMessageResourceType = typeof(Messages),ErrorMessageResourceName = nameof(Messages.InvalidPasswordFormat))]
        public string Password { get; set; }
        [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = nameof(Messages.FieldRequired))]
        [Compare(nameof(Password),ErrorMessageResourceType = typeof(Messages),ErrorMessageResourceName = nameof(Messages.PasswordsDoNotMatch))]
        public string ConfirmPassword { get; set; }
        [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = nameof(Messages.FieldRequired))]
        public string DisplayName { get; set; }
        [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = nameof(Messages.FieldRequired))]
        public string PhoneNumber { get; set; }
        public long? ManagerId { get; set; }
        [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = nameof(Messages.FieldRequired))]
        public long DepartmentId { get; set; }
        [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = nameof(Messages.FieldRequired))]

        public long RoleId { get; set; }
        [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = nameof(Messages.FieldRequired))]
        public string DeviceName { get; set; }
        [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = nameof(Messages.FieldRequired))]
        public string CertificatePassword { get; set; }
    }
}
