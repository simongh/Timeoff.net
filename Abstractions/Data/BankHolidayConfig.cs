using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Timeoff.Data
{
    internal class BankHolidayConfig : IEntityTypeConfiguration<Entities.BankHoliday>
    {
        public void Configure(EntityTypeBuilder<Entities.BankHoliday> builder)
        {
            builder.ToTable("BankHolidays");
        }
    }
}