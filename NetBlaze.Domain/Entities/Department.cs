using NetBlaze.Domain.Common;
using NetBlaze.Domain.Entities.Identity;

namespace NetBlaze.Domain.Entities
{
    public class Department : BaseEntity<long>
    {
        public string DepartmentName { get; set; }

        public virtual ICollection<User> Users { get; set; } = [];

    }
}
