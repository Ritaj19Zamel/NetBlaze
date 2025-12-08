using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetBlaze.Domain.Entities;

namespace NetBlaze.Infrastructure.Data.Configurations.EntitiesConfigurations
{
    public class PolicyEntityConfiguration : IEntityTypeConfiguration<Policy>
    {
        public void Configure(EntityTypeBuilder<Policy> builder)
        {
            builder.HasIndex(p => p.PolicyCode)
                .IsUnique();
        }
    }
}
