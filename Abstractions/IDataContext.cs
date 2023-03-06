using Microsoft.EntityFrameworkCore;
using Timeoff.Entities;

namespace Timeoff
{
    public interface IDataContext
    {
        DbSet<Audit> Audits { get; set; }
        DbSet<BankHoliday> BankHolidays { get; set; }
        DbSet<Comment> Comments { get; set; }
        DbSet<Company> Companies { get; set; }
        DbSet<Department> Departments { get; set; }
        DbSet<DepartmentSupervisor> DepartmentsSupervisors { get; set; }
        DbSet<EmailAudit> EmailAudits { get; set; }
        DbSet<UserFeed> Feeds { get; set; }
        DbSet<Leave> Leaves { get; set; }
        DbSet<LeaveType> LeaveTypes { get; set; }
        DbSet<Schedule> Schedules { get; set; }
        DbSet<UserAllowanceAdjustment> UserAllowanceAdjustments { get; set; }
        DbSet<User> Users { get; set; }

        //DatabaseFacade Database { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}