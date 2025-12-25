

using NetBlaze.Domain.Common;
using NetBlaze.SharedKernel.Enums;

namespace NetBlaze.Domain.Entities
{
    public class Permission : BaseEntity<long>
    {
        public string Name { get; set; }           
        public string DisplayName { get; set; }    
        public string Path { get; set; }           
        public string HttpMethod { get; set; }     
        public string? Description { get; set; }  

        public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
    }
}
