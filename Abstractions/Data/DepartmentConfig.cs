using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Timeoff.Data
{
    internal class DepartmentConfig : IEntityTypeConfiguration<Entities.Team>
    {
        public void Configure(EntityTypeBuilder<Entities.Team> builder)
        {
            builder.ToTable("Departments");

            builder.HasMany(p => p.Supervisors)
                .WithMany(p => p.DepartmentsSupervised)
                .UsingEntity(j => j.ToTable("DepartmentSupervisors"));

            builder.HasMany(p => p.Users)
                .WithOne(p => p.Department);
        }
    }
}