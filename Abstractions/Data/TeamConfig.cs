using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Timeoff.Data
{
    internal class TeamConfig : IEntityTypeConfiguration<Entities.Team>
    {
        public void Configure(EntityTypeBuilder<Entities.Team> builder)
        {
            builder.ToTable("Teams");

            builder.HasMany(p => p.Approvers)
                .WithMany(p => p.TeamApprover)
                .UsingEntity(j => j.ToTable("TeamApprovers"));

            builder.HasMany(p => p.Users)
                .WithOne(p => p.Team);
        }
    }
}