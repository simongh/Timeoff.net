using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Timeoff.Data
{
    internal class LeaveTypeConfig : IEntityTypeConfiguration<Entities.LeaveType>
    {
        public void Configure(EntityTypeBuilder<Entities.LeaveType> builder)
        {
            builder.ToTable("LeaveTypes");
        }
    }
}