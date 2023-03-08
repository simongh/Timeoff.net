using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Timeoff.Entities;

namespace Timeoff.Data
{
    internal class ScheduleConfig : IEntityTypeConfiguration<Schedule>
    {
        public void Configure(EntityTypeBuilder<Entities.Schedule> builder)
        {
            builder.ToTable("Schedules");

            builder.HasOne(p => p.Company)
                .WithOne(p => p.Schedule);
        }
    }
}