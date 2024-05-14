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
            var holidays = await _dataContext.Calendar
                .Where(h => h.IsHoliday)
                .Where(h => h.Date >= request.StartDate && h.Date <= request.EndDate)
                .Select(h => h.Date)
                .ToArrayAsync();

            var query = _dataContext.Users.AsQueryable();

            if (request.Team.HasValue)
                query = query.Where(u => u.TeamId == request.Team.Value);

            return await query
                .Select(u => new UserSummaryResult
                {
                    LeaveSummary = u.Calendar
                        .Where(c => c.Date >= request.StartDate && c.Date <= request.EndDate)
                        .Select(c => new
                        {
                            c.LeaveTypeId,
                            Day = c.LeavePart == LeavePart.All ? 1 : 0.5
                        })
                        .GroupBy(c => c.LeaveTypeId)
                        .Select(c => new LeaveSummaryResult
                        {
                            Id = c.Key.Value,
                            AllowanceUsed = c.Sum(l => l.Day)
                        }),
                    AllowanceUsed = u.Calendar
                        .Where(c => c.Date >= request.StartDate && c.Date <= request.EndDate)
                        .Where(c => c.LeaveType!.UseAllowance)
                        .Sum(c => c.LeavePart == LeavePart.All ? 1 : 0.5),
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Id = u.UserId,
                    IsActive = u.IsActive,
                })
                .OrderBy(l => l.FirstName)
                .AsNoTracking()
                .ToArrayAsync();
        }
    }
}