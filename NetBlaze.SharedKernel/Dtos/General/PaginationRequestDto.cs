

namespace NetBlaze.SharedKernel.Dtos.General
{
    public sealed record PaginationRequestDto
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
