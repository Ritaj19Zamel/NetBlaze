namespace NetBlaze.SharedKernel.Dtos.Department.Requests
{
    public sealed record UpdateDepartmentRequestDto
    {
        public long Id { get; set; }
        public string Name { get; set; } = null!;
    }
}
