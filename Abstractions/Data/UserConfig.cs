using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Timeoff.Data
{
    internal class UserConfig : IEntityTypeConfiguration<Entities.User>
    {
        public void Configure(EntityTypeBuilder<Entities.User> builder)
        {
            builder.ToTable("Users");

            builder
                .HasMany(p => p.ManagedDepartments)
                .WithOne(p => p.Manager);

            builder
                .HasOne(p => p.Schedule)
                .WithOne(p => p.User);

            builder
                .HasMany(p => p.DepartmentsSupervised)
                .WithMany(p => p.Supervisors)
                .UsingEntity(p => p.ToTable("DepartmentSupervisors"));
        }
    }
}