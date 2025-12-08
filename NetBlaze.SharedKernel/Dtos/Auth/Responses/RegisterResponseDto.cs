
namespace NetBlaze.SharedKernel.Dtos.Auth.Responses
{
    public sealed record RegisterResponseDto
    {
        public long UserId { get; set; }
        public string Email { get; set; } 
        public string DisplayName { get; set; } 
        public long DepartmentId { get; set; }
        public long RoleId { get; set; }
        public long? ManagerId { get; set; }
    }
}
