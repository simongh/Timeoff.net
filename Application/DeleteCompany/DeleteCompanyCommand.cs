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

            using var tx = _dataContext.BeginTransaction();
            try
            {
                await _dataContext.Schedules.ExecuteDeleteAsync();
                await _dataContext.EmailAudits.ExecuteDeleteAsync();
                await _dataContext.Leaves.ExecuteDeleteAsync();
                await _dataContext.LeaveTypes.ExecuteDeleteAsync();

                await _dataContext.Teams.ExecuteUpdateAsync(m => m.SetProperty(p => p.ManagerId, (int?)null));

                await _dataContext.Users.ExecuteDeleteAsync();
                await _dataContext.Teams.ExecuteDeleteAsync();
                await _dataContext.Calendar.ExecuteDeleteAsync();

                await _dataContext.Companies
                    .Where(c => c.CompanyId == _currentUserService.CompanyId)
                    .ExecuteDeleteAsync();

                await tx.CommitAsync();
                return new();
            }
            catch
            {
                return new()
                {
                    Errors = ["Unable to delete company. A database error occurred"]
                };
            }
        }
    }
}