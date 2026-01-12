

using NetBlaze.Domain.Common;
using NetBlaze.Domain.Entities.Identity;

namespace NetBlaze.Domain.Entities
{
    public class UserDevice : BaseEntity<long>
    {
        public long UserId { get; set; }

        public string DeviceName { get; set; } = null!;

        public byte[] CredentialId { get; set; } = null!;
        public byte[] PublicKey { get; set; } = null!;
        public uint SignatureCounter { get; set; }

        public string CredType { get; set; } = null!;
        public Guid AaGuid { get; set; }

        public DateTimeOffset? RevokedAt { get; set; }
        public string? RevokedBy { get; set; }
        public string? RevokeReason { get; set; }

        public virtual User User { get; set; }
        public string CredentialIdBase64 { get; set; }
    }
}
