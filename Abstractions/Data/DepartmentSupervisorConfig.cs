using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Timeoff.Data
{
    internal class DepartmentSupervisorConfig : IEntityTypeConfiguration<Entities.DepartmentSupervisor>
    {
        public void Configure(EntityTypeBuilder<Entities.DepartmentSupervisor> builder)
        {
            builder.ToTable("DepartmentSupervisors");

            builder
                .HasKey(p => new
                {
                    p.UserId,
                    p.DepartmentId,
                });
        }
    }
}