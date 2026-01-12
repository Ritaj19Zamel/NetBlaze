

using NetBlaze.SharedKernel.HelperUtilities.Constants;
using NetBlaze.SharedKernel.SharedResources;
using System.ComponentModel.DataAnnotations;

namespace NetBlaze.SharedKernel.Dtos.User.Requests
{
    public sealed record EditProfileRequestDto
    {
        public string DisplayName { get; set; } = string.Empty;

        [RegularExpression(RegexTemplate.Email, ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = nameof(Messages.InvalidEmailFormat))]
        public string Email { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public string? CurrentPassword { get; set; }
        public string? NewPassword { get; set; }
        [Compare(nameof(NewPassword), ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = nameof(Messages.PasswordsDoNotMatch))]
        public string? ConfirmNewPassword { get; set; }
    }
}
