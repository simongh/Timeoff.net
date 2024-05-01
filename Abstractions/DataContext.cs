using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Timeoff.Services;

namespace Timeoff
{
    internal class DataContext(
        DbContextOptions options,
        Services.ICurrentUserService currentUserService)
        : DbContext(options), IDataContext
    {
        private readonly ICurrentUserService _currentUserService = currentUserService;

        //public DbSet<Entities.Audit> Audits { get; set; }

        public DbSet<Entities.PublicHoliday> PublicHolidays { get; set; }

        //public DbSet<Entities.Comment> Comments { get; set; }

        public DbSet<Entities.Company> Companies { get; set; }

        public DbSet<Entities.Team> Teams { get; set; }

        public DbSet<Entities.EmailAudit> EmailAudits { get; set; }

        public DbSet<Entities.Leave> Leaves { get; set; }

        public DbSet<Entities.LeaveType> LeaveTypes { get; set; }

        public DbSet<Entities.Schedule> Schedules { get; set; }

        public DbSet<Entities.User> Users { get; set; }

        public DbSet<Entities.UserAllowanceAdjustment> UserAllowanceAdjustments { get; set; }

        public DbSet<Entities.UserFeed> Feeds { get; set; }

        public DbSet<Entities.Calendar> Calendar { get; set; }

        //public DataContext()
        //{ }

        //public DataContext(DbContextOptions options) : base(options)
        //{ }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .ApplyConfigurationsFromAssembly(typeof(DataContext).Assembly);

            modelBuilder.Entity<Entities.Calendar>().HasQueryFilter(m => m.CompanyId == _currentUserService.CompanyId);
            modelBuilder.Entity<Entities.EmailAudit>().HasQueryFilter(m => m.CompanyId == _currentUserService.CompanyId);
            modelBuilder.Entity<Entities.LeaveType>().HasQueryFilter(m => m.CompanyId == _currentUserService.CompanyId);
            modelBuilder.Entity<Entities.User>().HasQueryFilter(m => m.CompanyId == _currentUserService.CompanyId);
            modelBuilder.Entity<Entities.Schedule>().HasQueryFilter(m => m.CompanyId == _currentUserService.CompanyId);
            modelBuilder.Entity<Entities.Team>().HasQueryFilter(m => m.CompanyId == _currentUserService.CompanyId);
            modelBuilder.Entity<Entities.Leave>().HasQueryFilter(m => m.User.CompanyId == _currentUserService.CompanyId);
            modelBuilder.Entity<Entities.UserAllowanceAdjustment>().HasQueryFilter(m => m.User.CompanyId == _currentUserService.CompanyId);

            base.OnModelCreating(modelBuilder);
        }

        public IDbContextTransaction BeginTransaction() => base.Database.BeginTransaction();
    }
}