using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Timeoff.Application.Teams
{
    public record TeamsQuery : IRequest<IEnumerable<TeamResult>>
    {
    }

    internal class TeamsQueryHandler(
        IDataContext dataContext,
        Services.ICurrentUserService currentUserService)
        : IRequestHandler<TeamsQuery, IEnumerable<TeamResult>>
    {
        private readonly IDataContext _dataContext = dataContext;
        private readonly Services.ICurrentUserService _currentUserService = currentUserService;

        public async Task<IEnumerable<TeamResult>> Handle(TeamsQuery request, CancellationToken cancellationToken)
        {
            return await _dataContext.Teams
                 .Where(d => d.CompanyId == _currentUserService.CompanyId)
                 .OrderBy(d => d.Name)
                 .Select(d => new TeamResult
                 {
                     Id = d.TeamId,
                     Name = d.Name,
                     Allowance = d.Allowance,
                     EmployeeCount = d.Users.Count(),
                     IsAccruedAllowance = d.IsAccrued,
                     IncludePublicHolidays = d.IncludePublicHolidays,
                     ManagerId = d.ManagerId!.Value,
                     Manager = new()
                     {
                         Id = d.ManagerId!.Value,
                         Name = d.Manager!.FirstName + " " + d.Manager.LastName
                     }
                 })
                 .ToArrayAsync();
        }
    }
}