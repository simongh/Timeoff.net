using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Timeoff.Data
{
    internal class LeaveConfig : IEntityTypeConfiguration<Entities.Leave>
    {
        public void Configure(EntityTypeBuilder<Entities.Leave> builder)
        {
            builder.ToTable("Leaves");

            builder
                .HasOne(p => p.Approver)
                .WithMany();

            builder
                .Property(p => p.CreatedAt)
                .HasDefaultValue(DateTime.UtcNow);
        }
    }
}