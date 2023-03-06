using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Timeoff.Data
{
    internal class DepartmentConfig : IEntityTypeConfiguration<Entities.Department>
    {
        public void Configure(EntityTypeBuilder<Entities.Department> builder)
        {
            builder.ToTable("Departments");
        }
    }
}