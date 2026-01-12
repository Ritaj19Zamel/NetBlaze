
namespace NetBlaze.SharedKernel.Dtos.RandomCheck.Requests
{
    public sealed record GenerateOtpRequestDto
    {
        public bool SendToAllEmployees { get; set; }

        public List<long> UserIds { get; set; } = new();

    }
}
