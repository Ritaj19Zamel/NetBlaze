namespace NetBlaze.SharedKernel.Dtos.User.Responses
{
    public sealed record GetUserResponseDto
    {
        public long Id { get; set; }
        public string UserName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string DisplayName { get; set; } = null!;
        public string Role { get; set; } = null!;
        public string Department { get; set; } = null!;
        public long RoleId { get; set; }
        public long DepartmentId { get; set; }
        public long? ManagerId { get; set; }

    }
}
