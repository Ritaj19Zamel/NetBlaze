
namespace NetBlaze.SharedKernel.Dtos.User.Responses
{
    public sealed record GetUserProfileResponseDto
    {
        public long Id { get; set; }
        public string DisplayName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
    }
}
