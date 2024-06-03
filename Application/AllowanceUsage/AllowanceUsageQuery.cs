using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Timeoff.Application.AllowanceUsage
{
    public record AllowanceUsageQuery : IRequest<IEnumerable<UserSummaryResult>>
    {
        public DateTime StartDate { get; init; } = DateTime.Today.AddDays(-DateTime.Today.Day + 1);

        public DateTime EndDate { get; init; } = DateTime.Today.AddMonths(1).AddDays(-DateTime.Today.Day + 1);

        public int? Team { get; init; }
    }

    internal class AllowanceByTimeQueryHandler(
        IDataContext dataContext,
        Services.IDaysCalculator daysCalculator)
        : IRequestHandler<AllowanceUsageQuery, IEnumerable<UserSummaryResult>>
    {
        private readonly IDataContext _dataContext = dataContext;
        private readonly Services.IDaysCalculator _daysCalculator = daysCalculator;

        public async Task<IEnumerable<UserSummaryResult>> Handle(AllowanceUsageQuery request, CancellationToken cancellationToken)
        {
            var query = _dataContext.Users.AsQueryable();

            if (request.Team.HasValue)
                query = query.Where(u => u.TeamId == request.Team.Value);

            var results = await query
                .Select(u => new
                {
                    Calendar = u.Calendar
                        .Where(c => c.Date >= request.StartDate && c.Date <= request.EndDate)
                        .Select(c => new
                        {
                            c.LeaveTypeId,
                            c.LeaveType.UseAllowance,
                            Day = c.LeavePart == LeavePart.All ? 1 : 0.5,
                        }),
                    //LeaveSummary = u.Calendar
                    //    .Where(c => c.Date >= request.StartDate && c.Date <= request.EndDate)
                    //    .Select(c => new
                    //    {
                    //        c.LeaveTypeId,
                    //        Day = c.LeavePart == LeavePart.All ? 1 : 0.5
                    //    })
                    //    .GroupBy(c => c.LeaveTypeId)
                    //    .Select(c => new LeaveSummaryResult
                    //    {
                    //        Id = c.Key.Value,
                    //        AllowanceUsed = c.Sum(l => l.Day)
                    //    }),
                    //AllowanceUsed = u.Calendar
                    //    .Where(c => c.Date >= request.StartDate && c.Date <= request.EndDate)
                    //    .Where(c => c.LeaveType!.UseAllowance)
                    //    .Sum(c => c.LeavePart == LeavePart.All ? 1 : 0.5),
                    u.FirstName,
                    u.LastName,
                    Id = u.UserId,
                    u.IsActive,
                })
                .OrderBy(l => l.FirstName)
                .AsNoTracking()
                .ToArrayAsync();

            return results.Select(u => new UserSummaryResult
            {
                LeaveSummary = u.Calendar
                    .GroupBy(c => c.LeaveTypeId)
                    .Select(c => new LeaveSummaryResult
                    {
                        Id = c.Key.Value,
                        AllowanceUsed = c.Sum(l => l.Day)
                    }),
                AllowanceUsed = u.Calendar
                    .Where(c => c.UseAllowance)
                    .Sum(l => l.Day),
                FirstName = u.FirstName,
                LastName = u.LastName,
                Id = u.Id,
                IsActive = u.IsActive,
            });
        }
    }
}