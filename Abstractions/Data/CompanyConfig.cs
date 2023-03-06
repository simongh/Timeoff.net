using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Timeoff.Data
{
    internal class CompanyConfig : IEntityTypeConfiguration<Entities.Company>
    {
        public void Configure(EntityTypeBuilder<Entities.Company> builder)
        {
            builder.ToTable("Companies");
        }
    }
}