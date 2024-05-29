using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Timeoff.Application.AbsenceSummary
{
    public record AbsenceSummaryQuery : IRequest
    {
        public DateTime Start { get; init; }

        public DateTime End { get; init; }

        public int? Team { get; init; }

        public int? LeaveType { get; init; }
    }

    internal class AbsenceSummaryQueryHandler(IDataContext dataContext) : IRequestHandler<AbsenceSummaryQuery>
    {
        private readonly IDataContext _dataContext = dataContext;

        public async Task Handle(AbsenceSummaryQuery request, CancellationToken cancellationToken)
        {
            var query = _dataContext.Leaves
                .Where(c => c.DateStart <= request.End || c.DateEnd >= request.Start);

            if (request.Team.HasValue)
            {
                query = query.Where(c => c.User!.TeamId == request.Team.Value);
            }

            if (request.LeaveType.HasValue)
            {
                query = query.Where(c => c.LeaveTypeId == request.LeaveType.Value);
            }

            var result = await query
                .OrderBy(l => l.User!.FirstName)
                .ThenBy(l => l.User!.LastName)
                .ThenByDescending(l => l.DateStart)
                .Select(l => new
                {
                    User = new ResultModels.ListResult
                    {
                        Id = l.UserId,
                        Name = l.User!.FirstName + " " + l.User.LastName,
                    },
                    Team = new ResultModels.ListResult
                    {
                        Id = l.User.TeamId,
                        Name = l.User.Team!.Name,
                    },
                    Manager = new ResultModels.ListResult
                    {
                        Id = l.User.Team!.ManagerId!.Value,
                        Name = l.User.Team!.Manager!.FirstName + " " + l.User.Team.Manager.LastName,
                    },
                    Start = l.DateStart,
                    End = l.DateEnd,
                    Days = l.Calendar.Sum(c => c.LeavePart == LeavePart.All ? 1 : 0.5),
                    l.Status,
                    Added = l.CreatedAt.Date,
                    Comment = l.EmployeeComment,
                })
                .ToArrayAsync();
        }
    }
}