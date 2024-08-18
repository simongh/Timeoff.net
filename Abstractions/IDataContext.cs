using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Timeoff.Entities;

namespace Timeoff
{
    public interface IDataContext
    {
        DbSet<Company> Companies { get; }

        DbSet<Team> Teams { get; }
        DbSet<EmailAudit> EmailAudits { get; }
        DbSet<UserFeed> Feeds { get; }
        DbSet<Leave> Leaves { get; }
        DbSet<LeaveType> LeaveTypes { get; }
        DbSet<Schedule> Schedules { get; }
        DbSet<UserAllowanceAdjustment> UserAllowanceAdjustments { get; }
        DbSet<User> Users { get; }
        DbSet<Calendar> Calendar { get; }

        IDbContextTransaction BeginTransaction();

        //DatabaseFacade Database { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}