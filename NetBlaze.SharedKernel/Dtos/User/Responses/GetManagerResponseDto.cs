namespace NetBlaze.SharedKernel.Dtos.User.Responses
{
    public sealed record GetManagerResponseDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
    }
}
