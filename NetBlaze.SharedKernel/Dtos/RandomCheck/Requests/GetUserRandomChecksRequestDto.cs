using NetBlaze.SharedKernel.Dtos.General;

namespace NetBlaze.SharedKernel.Dtos.RandomCheck.Requests
{
    public sealed record GetUserRandomChecksRequestDto
    {
        public long UserId { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }

        public PaginationRequestDto Pagination { get; set; } = new();
    }
}
