﻿using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Timeoff.Application.Teams
{
    public record GetTeamCommand : IRequest<TeamResult>, Commands.IValidated
    {
        public int Id { get; init; }

        public IEnumerable<ValidationFailure>? Failures { get; set; }
    }

    internal class GetTeamCommandHandler(
        IDataContext dataContext,
        Services.ICurrentUserService currentUserService)
        : IRequestHandler<GetTeamCommand, TeamResult>
    {
        private readonly IDataContext _dataContext = dataContext;
        private readonly Services.ICurrentUserService _currentUserService = currentUserService;

        public async Task<TeamResult> Handle(GetTeamCommand request, CancellationToken cancellationToken)
        {
            var team = await _dataContext.Teams
                .Where(d => d.TeamId == request.Id && d.CompanyId == _currentUserService.CompanyId)
                .Select(d => new TeamResult
                {
                    Id = d.TeamId,
                    Name = d.Name,
                    Allowance = d.Allowance,
                    IncludePublicHolidays = d.IncludePublicHolidays,
                    IsAccruedAllowance = d.IsAccrued,
                    Manager = new()
                    {
                        Id = d.ManagerId!.Value,
                        Name = d.Manager!.FirstName + " " + d.Manager!.LastName,
                    }
                })
                .FirstOrDefaultAsync()
                ?? throw new NotFoundException();

            return team;
        }
    }
}