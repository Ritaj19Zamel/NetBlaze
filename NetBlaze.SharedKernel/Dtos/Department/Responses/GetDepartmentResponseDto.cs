namespace NetBlaze.SharedKernel.Dtos.Department.Responses
{
    public sealed record GetDepartmentResponseDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
    }
}
