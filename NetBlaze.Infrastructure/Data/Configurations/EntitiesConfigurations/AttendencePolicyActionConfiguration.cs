

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetBlaze.Domain.Entities;

namespace NetBlaze.Infrastructure.Data.Configurations.EntitiesConfigurations
{
    public class AttendencePolicyActionConfiguration : IEntityTypeConfiguration<AttendencePolicyAction>
    {
        public void Configure(EntityTypeBuilder<AttendencePolicyAction> builder)
        {
            builder.HasOne(x => x.EmployeeAttendence)
               .WithMany(e => e.AttendencePolicyActions)
               .HasForeignKey(x => x.AttendenceId)
               .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Policy)
                   .WithMany(p => p.AttendencePolicyActions)
                   .HasForeignKey(x => x.PolicyId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
