using Microsoft.AspNetCore.Identity;

namespace NetBlaze.Domain.Entities.Identity
{
    public class User : IdentityUser<long>
    {
        // Properties

        public string DisplayName { get;  set; } = null!;

        public DateTimeOffset CreatedAt { get; private set; }

        public string? CreatedBy { get; private set; }

        public DateTimeOffset? LastModifiedAt { get; private set; }

        public string? LastModifiedBy { get; private set; }

        public DateTimeOffset? DeletedAt { get; private set; }

        public string? DeletedBy { get; private set; }

        public bool IsActive { get; private set; } = true;

        public bool IsDeleted { get; private set; }
        public long? ManagerId { get; set; }
        public long DepartmentId { get; set; }


        // Navigational Properties

        public ICollection<UserRole> UserRoles { get; private set; } = [];
        public virtual Department Department { get; set; }
        public virtual ICollection<EmployeeAttendence> Attendences { get; set; } = [];
        public virtual ICollection<RandomChecks> RandomChecks { get; set; } = [];
        public virtual User Manager { get; set; }
        public virtual ICollection<User> SubUsers { get; set; } = [];
        public ICollection<UserDevice> UserDevices { get; set; }

        public void SoftDelete()
        {
            IsDeleted = true;
            IsActive = false;
            DeletedAt = DateTimeOffset.UtcNow;
        }

    }
}
