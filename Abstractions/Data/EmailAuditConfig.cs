using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Timeoff.Data
{
    internal class EmailAuditConfig : IEntityTypeConfiguration<Entities.EmailAudit>
    {
        public void Configure(EntityTypeBuilder<Entities.EmailAudit> builder)
        {
            builder.ToTable("EmailAudits");
        }
    }
}