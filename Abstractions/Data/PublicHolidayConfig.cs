using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Timeoff.Data
{
    internal class PublicHolidayConfig : IEntityTypeConfiguration<Entities.PublicHoliday>
    {
        public void Configure(EntityTypeBuilder<Entities.PublicHoliday> builder)
        {
            builder.ToTable("PublicHolidays");
        }
    }
}