using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Timeoff.Data
{
    internal class UserAllowanceAdjustmentConfig : IEntityTypeConfiguration<Entities.UserAllowanceAdjustment>
    {
        public void Configure(EntityTypeBuilder<Entities.UserAllowanceAdjustment> builder)
        {
            builder.ToTable("UserAllowanceAdjustments");

            builder.Property(p => p.CreatedAt)
                .HasDefaultValue(DateTime.UtcNow);

            builder
                .Property(p => p.RowVersion)
                .IsRowVersion();
        }
    }
}