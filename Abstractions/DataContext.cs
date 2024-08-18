using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Timeoff.Services;

namespace Timeoff
{
    public class DataContext(
        DbContextOptions options,
        Services.ICurrentUserService currentUserService)
        : DbContext(options), IDataContext
    {
        private readonly ICurrentUserService _currentUserService = currentUserService;

        public DbSet<Entities.PublicHoliday> PublicHolidays => Set<Entities.PublicHoliday>();

        public DbSet<Entities.Company> Companies => Set<Entities.Company>();

        public DbSet<Entities.Team> Teams => Set<Entities.Team>();

        public DbSet<Entities.EmailAudit> EmailAudits => Set<Entities.EmailAudit>();

        public DbSet<Entities.Leave> Leaves => Set<Entities.Leave>();

        public DbSet<Entities.LeaveType> LeaveTypes => Set<Entities.LeaveType>();

        public DbSet<Entities.Schedule> Schedules => Set<Entities.Schedule>();

        public DbSet<Entities.User> Users => Set<Entities.User>();

        public DbSet<Entities.UserAllowanceAdjustment> UserAllowanceAdjustments => Set<Entities.UserAllowanceAdjustment>();

        public DbSet<Entities.UserFeed> Feeds => Set<Entities.UserFeed>();

        public DbSet<Entities.Calendar> Calendar => Set<Entities.Calendar>();

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