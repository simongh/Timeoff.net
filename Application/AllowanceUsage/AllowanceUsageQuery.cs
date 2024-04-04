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
        Services.ICurrentUserService currentUserService)
        : IRequestHandler<AllowanceUsageQuery, IEnumerable<UserSummaryResult>>
    {
        private readonly IDataContext _dataContext = dataContext;
        private readonly Services.ICurrentUserService _currentUserService = currentUserService;

        public async Task<IEnumerable<UserSummaryResult>> Handle(AllowanceUsageQuery request, CancellationToken cancellationToken)
        {
            var query = _dataContext.Users
                .Where(u => u.CompanyId == _currentUserService.CompanyId);

            if (request.Team.HasValue)
                query = query.Where(u => u.TeamId == request.Team.Value);

            var data = await query
                .Select(u => new
                {
                    Leaves = u.Leave
                        .Where(l => l.DateStart > request.StartDate || l.DateEnd > request.EndDate)
                        .Select(l => new LeaveSummary
                        {
                            Start = l.DateStart < request.StartDate ? request.StartDate : l.DateStart,
                            End = l.DateEnd > request.EndDate ? request.EndDate : l.DateEnd,
                            StartPart = l.DayPartStart,
                            EndPart = l.DayPartEnd,
                            Id = l.LeaveTypeId,
                            UsesAllowance = l.LeaveType.UseAllowance,
                        }),
                    u.FirstName,
                    u.LastName,
                    u.UserId,
                })
                .ToArrayAsync();

            return data.Select(d => new UserSummaryResult
            {
                FirstName = d.FirstName,
                LastName = d.LastName,
                Id = d.UserId,
                AllowanceUsed = d.Leaves
                    .Where(l => l.UsesAllowance)
                    .Sum(l => l.Days),
                LeaveSummary = d.Leaves
                    .GroupBy(d => d.Id)
                    .Select(d => new LeaveSummaryResult
                    {
                        Id = d.Key,
                        AllowanceUsed = d.Sum(l => l.Days),
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