using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetBlaze.Domain.Entities.Views;

namespace NetBlaze.Infrastructure.Data.Configurations.EntitiesConfigurations
{
    public class AttendanceViewEntityConfiguration : IEntityTypeConfiguration<AttendanceView>
    {
        public void Configure(EntityTypeBuilder<AttendanceView> builder)
        {
            builder.HasNoKey();
            builder.ToView("vw_attendancedailyreport");
        }
    }
}
