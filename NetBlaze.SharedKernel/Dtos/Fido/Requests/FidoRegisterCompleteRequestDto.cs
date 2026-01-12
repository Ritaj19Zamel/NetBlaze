using Fido2NetLib;
using System.Text.Json;

namespace NetBlaze.SharedKernel.Dtos.Fido.Requests
{
    public class FidoRegisterCompleteRequestDto
    {
        public long UserId { get; set; }
        public string AttestationJson { get; set; } = default!;

    }
}
