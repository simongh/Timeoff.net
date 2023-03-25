using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Timeoff
{
    internal class DataContext : DbContext, IDataContext
    {
        //public DbSet<Entities.Audit> Audits { get; set; }

        public DbSet<Entities.PublicHoliday> PublicHolidays { get; set; }

        //public DbSet<Entities.Comment> Comments { get; set; }

        public DbSet<Entities.Company> Companies { get; set; }

        public DbSet<Entities.Department> Departments { get; set; }

        public DbSet<Entities.EmailAudit> EmailAudits { get; set; }

        public DbSet<Entities.Leave> Leaves { get; set; }

        public DbSet<Entities.LeaveType> LeaveTypes { get; set; }

        public DbSet<Entities.Schedule> Schedules { get; set; }

        public DbSet<Entities.User> Users { get; set; }

        public DbSet<Entities.UserAllowanceAdjustment> UserAllowanceAdjustments { get; set; }

        public DbSet<Entities.UserFeed> Feeds { get; set; }

        public DataContext()
        { }

        public DataContext(DbContextOptions options) : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .ApplyConfigurationsFromAssembly(typeof(DataContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }

        public IDbContextTransaction BeginTransaction() => base.Database.BeginTransaction();
    }
}