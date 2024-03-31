using MediatR;
using Microsoft.EntityFrameworkCore;
using Timeoff.Entities;

namespace Timeoff.Application.Users
{
    public record UsersQuery : IRequest<IEnumerable<UserInfoResult>>
    {
        public int? Team { get; init; }

        public bool AsCsv { get; init; }
    }

    internal class UsersQueryHandler(
        IDataContext dataContext,
        Services.ICurrentUserService currentUserService)
        : IRequestHandler<UsersQuery, IEnumerable<UserInfoResult>>
    {
        private readonly IDataContext _dataContext = dataContext;
        private readonly Services.ICurrentUserService _currentUserService = currentUserService;

        public async Task<IEnumerable<UserInfoResult>> Handle(UsersQuery request, CancellationToken cancellationToken)
        {
            var query = dataContext.Users
                .Where(u => u.CompanyId == _currentUserService.CompanyId);

            if (request.Team.HasValue)
            {
                query = query.Where(u => u.TeamId == request.Team.Value);
            }
            var year = DateTime.Today.Year;

            return await query
                .OrderBy(u => u.FirstName)
                .ThenBy(u => u.LastName)
                .Select(u => new Users.UserInfoResult
                {
                    Id = u.UserId,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Team = new()
                    {
                        Id = u.TeamId,
                        Name = u.Team.Name,
                    },
                    IsActive = u.IsActive,
                    IsAdmin = u.IsAdmin,
                    AllowanceCalculator = new()
                    {
                        DaysUsed = u.Leave
                            .Where(a => a.DateStart.Year == year && a.DateEnd.Year == year)
                            .Where(a => a.Status == LeaveStatus.Approved)
                            .Sum(a => a.Days),
                        Start = u.StartDate,
                        End = u.EndDate,
                        IsAccrued = u.Team.IsAccrued,
                        Adjustment = u.Adjustments
                            .Where(a => a.Year == year)
                            .Any()
                            ? u.Adjustments
                                .Where(a => a.Year == year)
                                .FirstOrDefault()!
                                .Adjustment
                            : 0,
                        Allowance = u.Team.Allowance,
                    }
                })
                .ToArrayAsync();
        }
    }
}