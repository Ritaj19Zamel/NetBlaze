

using NetBlaze.Domain.Common;
using NetBlaze.Domain.Entities.Identity;

namespace NetBlaze.Domain.Entities
{
    public class UserDetail : BaseEntity<long>
    {
        public long UserId { get; set; }
        public string DeviceName { get; set; }
        public string CertificatePassword { get; set; }
        public virtual User User { get; set; }

    }
}
