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
               .Property(p => p.RowVersion)
               .IsRowVersion();

            builder
                 .HasMany(p => p.ManagedTeams)
                .WithOne(p => p.Manager);

            builder
                .HasOne(p => p.Schedule)
                .WithOne(p => p.User);

            builder
                .HasMany(p => p.TeamApprover)
                .WithMany(p => p.Approvers)
                .UsingEntity(p => p.ToTable("TeamApprovers"));
        }
    }
}