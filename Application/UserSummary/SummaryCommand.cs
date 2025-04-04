﻿using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Timeoff.Application.UserSummary
{
    public record SummaryCommand : IRequest<SummaryViewModel>
    {
        public int Id { get; init; }
    }

    internal class SummaryCommandHandler(
        IDataContext dataContext,
        Services.ICurrentUserService currentUserService)
        : IRequestHandler<SummaryCommand, SummaryViewModel>
    {
        private readonly IDataContext _dataContext = dataContext;
        private readonly Services.ICurrentUserService _currentUserService = currentUserService;

        public async Task<SummaryViewModel> Handle(SummaryCommand request, CancellationToken cancellationToken)
        {
            var dateStart = new DateTime(DateTime.Today.Year, 1, 1);
            var dateEnd = new DateTime(DateTime.Today.Year + 1, 1, 1);

            var adj = await _dataContext.UserAllowanceAdjustments
                .Where(a => a.Year == DateTime.Today.Year && a.UserId == request.Id)
                .FirstOrDefaultAsync()
                ?? new();

            var user = await _dataContext.Users
                .Where(u => u.UserId == request.Id)
                .Select(u => new SummaryViewModel
                {
                    Name = u.FirstName + " " + u.LastName,
                    Team = u.Team.Name,
                    Used = u.Leave.Where(l => l.LeaveType.UseAllowance && l.DateStart >= dateStart && l.DateEnd < dateEnd).Sum(l => l.Days),
                    Total = u.Team.Allowance + adj.CarriedOverAllowance + adj.Adjustment,
                    Approver = u.Team.Manager.FirstName + " " + u.Team.Manager.LastName,
                    ShowDetailed = u.Team.ManagerId == _currentUserService.UserId,
                })
                .FirstOrDefaultAsync()
                ?? throw new NotFoundException();

            return user;
        }
    }
}