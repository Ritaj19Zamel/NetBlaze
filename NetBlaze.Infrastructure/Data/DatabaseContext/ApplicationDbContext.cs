using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NetBlaze.Domain.Entities;
using NetBlaze.Domain.Entities.Identity;
using NetBlaze.Domain.Entities.Views;
using NetBlaze.Infrastructure.Data.Configurations.MiscConfigurations;
using System.Reflection;

namespace NetBlaze.Infrastructure.Data.DatabaseContext
{
    public class ApplicationDbContext : IdentityDbContext<User,
                                                          Role,
                                                          long,
                                                          IdentityUserClaim<long>,
                                                          UserRole,
                                                          IdentityUserLogin<long>,
                                                          IdentityRoleClaim<long>,
                                                          IdentityUserToken<long>>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }


        public DbSet<Department> Departments => Set<Department>();
        public DbSet<UserDetail> UserDetails => Set<UserDetail>();
        public DbSet<EmployeeAttendence> EmployeeAttendences => Set<EmployeeAttendence>();
        public DbSet<RandomChecks> RandomChecks => Set<RandomChecks>();
        public DbSet<Vacation> Vacations => Set<Vacation>();
        public DbSet<Policy> Policies => Set<Policy>();
        public DbSet<AttendanceView> AttendanceDailyReports { get; set; }

        public DbSet<AttendencePolicyAction> AttendencePolicyActions => Set<AttendencePolicyAction>();
        public DbSet<Permission> Permissions => Set<Permission>();
        public DbSet<RolePermission> RolePermissions => Set<RolePermission>();


        protected override void OnModelCreating(ModelBuilder builder)
        {
            
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
           builder.ConfigureIdentityTablesNames();
           builder.SetGlobalIsDeletedFilterToAllEntities();
          

        }
        
   
    }
}