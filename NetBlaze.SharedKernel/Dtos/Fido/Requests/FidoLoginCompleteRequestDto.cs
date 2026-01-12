

namespace NetBlaze.SharedKernel.Dtos.Fido.Requests
{
    public class FidoLoginCompleteRequestDto
    {
        public long UserId { get; set; }
        public string AttestationJson { get; set; } = default!;
    }
}
