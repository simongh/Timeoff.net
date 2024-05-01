using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Timeoff.Entities;

namespace Timeoff.Data
{
    internal class CalendarConfig : IEntityTypeConfiguration<Entities.Calendar>
    {
        public void Configure(EntityTypeBuilder<Calendar> builder)
        {
            builder.ToTable("calendar");

            builder
                .Property(p => p.RowVersion)
                .IsRowVersion();
        }
    }
}