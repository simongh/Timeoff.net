using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Timeoff.Application.Teams
{
    public record UpdateTeamCommand : TeamModel, IRequest<ResultModels.ApiResult>, Commands.IValidated
    {
        public int? Id { get; set; }

        public int Manager { get; init; }

        public IEnumerable<ValidationFailure>? Failures { get; set; }
    }

    internal class UpdateTeamCommandHandler(
        IDataContext dataContext,
        Services.ICurrentUserService currentUserService)
        : IRequestHandler<UpdateTeamCommand, ResultModels.ApiResult>
    {
        private readonly IDataContext _dataContext = dataContext;
        private readonly Services.ICurrentUserService _currentUserService = currentUserService;

        public async Task<ResultModels.ApiResult> Handle(UpdateTeamCommand request, CancellationToken cancellationToken)
        {
            var errors = new List<string>();

            if (request.Failures.IsValid())
            {
                var managerFound = await _dataContext.Users
                    .Where(u => u.UserId == request.Manager)
                    .AnyAsync();

                if (!managerFound)
                {
                    return new()
                    {
                        Errors = ["Invalid manager"],
                    };
                }

                Entities.Team? team;
                if (request.Id == null)
                {
                    team = new()
                    {
                        CompanyId = _currentUserService.CompanyId,
                    };
                    _dataContext.Teams.Add(team);
                }
                else
                {
                    team = await _dataContext.Teams
                        .Where(d => d.TeamId == request.Id.Value)
                        .FirstOrDefaultAsync();

                    if (team == null)
                        return new()
                        {
                            Errors = ["Invalid team"],
                        };
                }

                team.Name = request.Name;
                team.Allowance = request.Allowance;
                team.IsAccrued = request.IsAccruedAllowance;
                team.IncludePublicHolidays = request.IncludePublicHolidays;
                team.ManagerId = request.Manager;

                await _dataContext.SaveChangesAsync();
            }
            else
            {
                errors.AddRange(request.Failures.Select(v => v.ErrorMessage));
            }

            return new()
            {
                Errors = errors,
            };
        }
    }
}