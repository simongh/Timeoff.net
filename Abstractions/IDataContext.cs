﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Timeoff.Entities;

namespace Timeoff
{
    public interface IDataContext
    {
        //DbSet<Audit> Audits { get; set; }
        DbSet<PublicHoliday> PublicHolidays { get; set; }

        //DbSet<Comment> Comments { get; set; }
        DbSet<Company> Companies { get; set; }

        DbSet<Team> Teams { get; set; }
        DbSet<EmailAudit> EmailAudits { get; set; }
        DbSet<UserFeed> Feeds { get; set; }
        DbSet<Leave> Leaves { get; set; }
        DbSet<LeaveType> LeaveTypes { get; set; }
        DbSet<Schedule> Schedules { get; set; }
        DbSet<UserAllowanceAdjustment> UserAllowanceAdjustments { get; set; }
        DbSet<User> Users { get; set; }

        IDbContextTransaction BeginTransaction();

        //DatabaseFacade Database { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}