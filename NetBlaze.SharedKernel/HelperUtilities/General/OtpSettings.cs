

namespace NetBlaze.SharedKernel.HelperUtilities.General
{
    public sealed record OtpSettings
    {
        public int ExpiryInMinutes { get; set; }
        public string VerifyBaseUrl { get; set; } = null!;

    }
}
