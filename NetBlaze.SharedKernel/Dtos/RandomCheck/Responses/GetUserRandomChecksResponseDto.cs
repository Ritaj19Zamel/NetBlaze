
namespace NetBlaze.SharedKernel.Dtos.RandomCheck.Responses
{
    public sealed record GetUserRandomChecksResponseDto
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public string UserName { get; set; }
        public string OTP { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime ExpirationTime { get; set; }
        public DateTime? CheckDateTime { get; set; }
        public bool IsChecked { get; set; }
    }
}
