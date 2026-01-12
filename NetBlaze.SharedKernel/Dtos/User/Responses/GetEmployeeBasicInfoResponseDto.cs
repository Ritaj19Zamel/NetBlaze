

namespace NetBlaze.SharedKernel.Dtos.User.Responses
{
    public sealed record GetEmployeeBasicInfoResponseDto
    {
        public long Id { get; init; }
        public string UserName { get; init; } = string.Empty;
        public string Email { get; init; } = string.Empty;
    }
}
