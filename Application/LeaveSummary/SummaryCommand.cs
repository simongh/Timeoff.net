using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Timeoff.Application.LeaveSummary
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
            return await _dataContext.Leaves
                .Where(l => l.LeaveId == request.Id && l.User.CompanyId == _currentUserService.CompanyId)
                .Select(l => new SummaryViewModel
                {
                    StartDate = l.DateStart,
                    EndDate = l.DateEnd,
                    Name = l.User.FirstName + " " + l.User.LastName,
                    Approver = l.Approver.FirstName + " " + l.Approver.LastName,
                    Type = l.LeaveType.Name,
                    Comment = l.EmployeeComment,
                    DateFormat = l.User.Company.DateFormat,
                    Days = l.Days,
                    Status = l.Status,
                    ShowDetailed = _currentUserService.IsAdmin || l.User.Team.ManagerId == _currentUserService.UserId,
                })
                .FirstOrDefaultAsync()
                ?? throw new NotFoundException();
        }
    }
}