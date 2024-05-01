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

            var data = await query
                .Select(u => new
                {
                    Leaves = u.Leave
                        .Where(l => l.DateStart > request.StartDate || l.DateEnd > request.EndDate)
                        .Select(l => new
                        {
                            LeaveType = l.LeaveTypeId,
                            l.LeaveType.UseAllowance,
                            Leave = l,
                        }),
                    Schedule = u.Schedule ?? u.Company.Schedule,
                    u.FirstName,
                    u.LastName,
                    u.UserId,
                })
                .OrderBy(l => l.FirstName)
                .AsNoTracking()
                .ToArrayAsync();

            var normalised = data.Select(u => new
            {
                u.FirstName,
                u.LastName,
                u.UserId,
                Leaves = u.Leaves.Select(l =>
                {
                    if (request.StartDate > l.Leave.DateStart)
                    {
                        l.Leave.DateStart = request.StartDate;
                        l.Leave.DayPartStart = LeavePart.All;
                    }

                    if (l.Leave.DateEnd > request.EndDate)
                    {
                        l.Leave.DateEnd = request.EndDate;
                        l.Leave.DayPartEnd = LeavePart.All;
                    }

                    _daysCalculator.CalculateDays(l.Leave, u.Schedule, holidays);
                    return l;
                })
            });

            return normalised
                .Select(d => new UserSummaryResult
                {
                    FirstName = d.FirstName,
                    LastName = d.LastName,
                    Id = d.UserId,
                    AllowanceUsed = d.Leaves
                        .Where(l => l.UseAllowance)
                        .Sum(l => l.Leave.Days),
                    LeaveSummary = d.Leaves
                        .GroupBy(d => d.LeaveType)
                        .Select(d => new LeaveSummaryResult
                        {
                            Id = d.Key,
                            AllowanceUsed = d.Sum(l => l.Leave.Days),
                        }),
                })
                .ToArray();
        }

        private record LeaveSummary
        {
            public DateTime Start { get; init; }

            public DateTime End { get; init; }

            public LeavePart StartPart { get; init; }

            public LeavePart EndPart { get; init; }

            public int Id { get; init; }

            public bool UsesAllowance { get; init; }

            public double Days
            {
                get
                {
                    double result = End.Subtract(Start).Days;

                    if (StartPart == LeavePart.Morning)
                        result -= 0.5;

                    if (EndPart == LeavePart.Afternoon)
                        result -= 0.5;

                    return result;
                }
            }
        }
    }
}