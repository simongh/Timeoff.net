using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Timeoff.Data
{
    internal class UserConfig : IEntityTypeConfiguration<Entities.User>
    {
        public void Configure(EntityTypeBuilder<Entities.User> builder)
        {
            builder.ToTable("Users");
        }
    }
}