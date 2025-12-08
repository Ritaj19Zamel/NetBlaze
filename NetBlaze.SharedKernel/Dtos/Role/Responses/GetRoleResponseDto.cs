namespace NetBlaze.SharedKernel.Dtos.Role.Responses
{
    public sealed record GetRoleResponseDto
    {
        public long Id { get; set; }
        public string RoleName { get; set; }
    }
}
