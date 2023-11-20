using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Timeoff.Data
{
    internal class UserFeedConfig : IEntityTypeConfiguration<Entities.UserFeed>
    {
        public void Configure(EntityTypeBuilder<Entities.UserFeed> builder)
        {
            builder.ToTable("UserFeeds");

            builder
                .Property(p => p.RowVersion)
                .IsRowVersion();
        }
    }
}