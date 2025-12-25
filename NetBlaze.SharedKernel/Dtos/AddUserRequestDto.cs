using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBlaze.SharedKernel.Dtos
{
    public class AddUserRequestDto
    {
        public string DisplayName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string? PhoneNumber { get; set; }

        public long RoleId { get; set; }
        public long DepartmentId { get; set; }
        public long? ManagerId { get; set; }
    }
}
