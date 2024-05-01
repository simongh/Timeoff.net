using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Timeoff.Application.DeleteCompany
{
    public record DeleteCompanyCommand : IRequest<ResultModels.ApiResult>
    {
        public string? Name { get; init; }
    }

    internal class DeleteCompanyCommandHandler(
        IDataContext dataContext,
        Services.ICurrentUserService currentUserService)
        : IRequestHandler<DeleteCompanyCommand, ResultModels.ApiResult>
    {
        private readonly IDataContext _dataContext = dataContext;
        private readonly Services.ICurrentUserService _currentUserService = currentUserService;

        public async Task<ResultModels.ApiResult> Handle(DeleteCompanyCommand request, CancellationToken cancellationToken)
        {
            var match = await _dataContext.Companies
                .Where(c => c.Name == request.Name && c.CompanyId == _currentUserService.CompanyId)
                .AnyAsync();

            if (!match)
            {
                return new()
                {
                    Errors = ["The company name did not match"]
                };
            }

            var tx = _dataContext.BeginTransaction();
            try
            {
                var schedule = await _dataContext.Schedules
                    .Where(s => s.CompanyId == _currentUserService.CompanyId)
                    .ToArrayAsync();
                _dataContext.Schedules.RemoveRange(schedule);

                var emails = await _dataContext.EmailAudits
                    .Where(e => e.CompanyId == _currentUserService.CompanyId)
                    .ToArrayAsync();
                _dataContext.EmailAudits.RemoveRange(emails);

                var leaves = await _dataContext.Leaves
                    .Where(a => a.User.CompanyId == _currentUserService.CompanyId)
                    .ToArrayAsync();
                _dataContext.Leaves.RemoveRange(leaves);

                var leaveTypes = await _dataContext.LeaveTypes
                    .Where(t => t.CompanyId == _currentUserService.CompanyId)
                    .ToArrayAsync();
                _dataContext.LeaveTypes.RemoveRange(leaveTypes);

                var teams = await _dataContext.Teams
                    .Where(d => d.CompanyId == _currentUserService.CompanyId)
                    .ToArrayAsync();
                foreach (var d in teams)
                {
                    d.ManagerId = null;
                }
                await _dataContext.SaveChangesAsync();

                var users = await _dataContext.Users
                    .Where(u => u.CompanyId == _currentUserService.CompanyId)
                    .ToArrayAsync();
                _dataContext.Users.RemoveRange(users);

                _dataContext.Teams.RemoveRange(teams);

                var holidays = await _dataContext.Calendar
                    .Where(h => h.CompanyId == _currentUserService.CompanyId)
                    .ToArrayAsync();
                _dataContext.Calendar.RemoveRange(holidays);

                var company = await _dataContext.Companies
                    .Where(c => c.CompanyId == _currentUserService.CompanyId)
                    .ToArrayAsync();
                _dataContext.Companies.RemoveRange(company);

                await _dataContext.SaveChangesAsync();

                await tx.CommitAsync();
                return new();
            }
            catch
            {
                tx.Dispose();

                return new()
                {
                    Errors = ["Unable to delete company. A database error occurred"]
                };
            }
        }
    }
}