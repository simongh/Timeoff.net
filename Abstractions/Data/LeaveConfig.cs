using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Timeoff.Data
{
    internal class LeaveConfig : IEntityTypeConfiguration<Entities.Leave>
    {
        public void Configure(EntityTypeBuilder<Entities.Leave> builder)
        {
            builder.ToTable("Leaves");
        }
    }
}