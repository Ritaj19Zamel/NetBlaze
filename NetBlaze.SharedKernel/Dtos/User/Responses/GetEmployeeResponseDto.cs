
namespace NetBlaze.SharedKernel.Dtos.User.Responses
{
    public sealed record GetEmployeeResponseDto
    {
        public long Id { get; set; }
        public string DisplayName { get; set; } = string.Empty;
    }
}
