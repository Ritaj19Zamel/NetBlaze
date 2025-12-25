using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetBlaze.Domain.Entities.Views;


namespace NetBlaze.Infrastructure.Data.Configurations.EntitiesConfigurations
{
    internal class CheckInViolationViewEntityConfiguration : IEntityTypeConfiguration<CheckInViolationView>
    {
        public void Configure(EntityTypeBuilder<CheckInViolationView> builder)
        {
            builder.HasNoKey();
            builder.ToView("vw_employeecheckinviolations");
        }
    }
}
