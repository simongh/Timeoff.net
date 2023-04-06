using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Timeoff.Application.Teams
{
    public record GetTeamCommand : IRequest<EditTeamViewModel?>, Commands.IValidated
    {
        public int Id { get; init; }

        public IEnumerable<ValidationFailure>? Failures { get; set; }
    }

    internal class GetTeamCommandHandler : IRequestHandler<GetTeamCommand, EditTeamViewModel?>
    {
        private readonly IDataContext _dataContext;
        private readonly Services.ICurrentUserService _currentUserService;

        public GetTeamCommandHandler(
            IDataContext dataContext,
            Services.ICurrentUserService currentUserService)
        {
            _dataContext = dataContext;
            _currentUserService = currentUserService;
        }

        public async Task<EditTeamViewModel?> Handle(GetTeamCommand request, CancellationToken cancellationToken)
        {
            var team = await _dataContext.Teams
                .Where(d => d.TeamId == request.Id && d.CompanyId == _currentUserService.CompanyId)
                .Select(d => new EditTeamViewModel
                {
                    Id = d.TeamId,
                    Name = d.Name,
                    Allowance = d.Allowance,
                    IncludePublicHolidays = d.IncludePublicHolidays,
                    IsAccruedAllowance = d.IsAccrued,
                    ManagerId = d.ManagerId!.Value,
                    Users = d.Company.Users.Select(u => new ResultModels.ListItem
                    {
                        Id = u.UserId,
                        Value = u.FirstName + " " + u.LastName,
                    })
                })
                .FirstOrDefaultAsync();

            return team;
        }
    }
}