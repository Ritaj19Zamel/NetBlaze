

using NetBlaze.SharedKernel.HelperUtilities.Constants;
using NetBlaze.SharedKernel.SharedResources;
using System.ComponentModel.DataAnnotations;

namespace NetBlaze.SharedKernel.Dtos.Auth.Requests
{
    public class ResetPasswordRequestDto
    {
        [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = nameof(Messages.FieldRequired))]
        [RegularExpression(RegexTemplate.Email, ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = nameof(Messages.InvalidEmailFormat))]
        public string Email { get; set; }
        [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = nameof(Messages.FieldRequired))]
        [RegularExpression(RegexTemplate.Password, ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = nameof(Messages.InvalidPasswordFormat))]
        public string Password { get; set; }
        [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = nameof(Messages.FieldRequired))]
        [Compare(nameof(Password), ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = nameof(Messages.PasswordsDoNotMatch))]
        public string ConfirmPassword { get; set; }
        public string Token { get; set; }
    }
}
