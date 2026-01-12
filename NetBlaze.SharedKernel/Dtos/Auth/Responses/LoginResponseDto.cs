

namespace NetBlaze.SharedKernel.Dtos.Auth.Responses
{
    public sealed record LoginResponseDto
    {
        public long UserId { get; set; }
        public string DisplayName { get; set; } 
        public string Email { get; set; }
        public string Token { get; set; }
        public DateTime ExpirationTime { get; set; }
        public bool RequiresDeviceRegistration { get; set; }
        public bool RequiresDeviceAuthentication { get; set; }

    }

}
