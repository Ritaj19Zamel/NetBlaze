namespace NetBlaze.SharedKernel.Dtos.User.Responses
{
    public sealed record GetUserResponseDto
    {
        public long Id { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string PhoneNumber { get; set; }
        public long ManagerId { get; set; }
        public long DepartmentId { get; set; }
        public long RoleId { get; set; }
        public string DeviceName { get; set; }

    }
}
