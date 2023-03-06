using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Timeoff.Data
{
    internal class CommentConfig : IEntityTypeConfiguration<Entities.Comment>
    {
        public void Configure(EntityTypeBuilder<Entities.Comment> builder)
        {
            builder.ToTable("Comments");
        }
    }
}