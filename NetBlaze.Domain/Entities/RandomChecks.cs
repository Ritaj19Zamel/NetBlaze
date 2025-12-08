using NetBlaze.Domain.Common;
using NetBlaze.Domain.Entities.Identity;

namespace NetBlaze.Domain.Entities
{
    public class RandomChecks : BaseEntity<long>
    {
        public long UserId { get; set; }
        public DateTime CheckDateTime { get; set; }
        public DateTime ExpirationTime { get; set; }
        public DateTime CreationTIme { get; set; }
        public string OTP {  get; set; }
        public bool Ischecked { get; set; } 
        public virtual User User { get; set; }
    }
}
