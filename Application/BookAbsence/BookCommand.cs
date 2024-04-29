using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Timeoff.Application.BookAbsence
{
    public record BookCommand : IRequest<ResultModels.ApiResult>, Commands.IValidated
    {
        public int? Employee { get; init; }

        public int LeaveType { get; init; }

        public LeavePart StartPart { get; init; }

        public DateTime Start { get; init; }

        public LeavePart EndPart { get; init; }

        public DateTime End { get; init; }

        public string? Comment { get; init; }
        public IEnumerable<ValidationFailure>? Failures { get; set; }
    }

    internal class BookCommandHandler(
        IDataContext dataContext,
        Services.ICurrentUserService currentUserService,
        Services.INewLeaveService leaveService,
        Services.IDaysCalculator daysCalculator,
        IMediator mediator)
        : IRequestHandler<BookCommand, ResultModels.ApiResult>
    {
        private readonly IDataContext _dataContext = dataContext;
        private readonly Services.ICurrentUserService _currentUserService = currentUserService;
        private readonly Services.INewLeaveService _leaveService = leaveService;
        private readonly Services.IDaysCalculator _daysCalculator = daysCalculator;
        private readonly IMediator _mediator = mediator;

        public async Task<ResultModels.ApiResult> Handle(BookCommand request, CancellationToken cancellationToken)
        {
            if (!request.Failures.IsValid())
                return new()
                {
                    Errors = request.Failures.Select(e => e.ErrorMessage)
                };

            if (request.Employee != null)
            {
                if (!(await _leaveService.EmployeesIManageAsync()).Any(i => i.Id == request.Employee))
                {
                    return new()
                    {
                        Errors = ["The  employee is not managed by the current user"]
                    };
                }
            }
            var leaveType = await _dataContext.LeaveTypes
                .Where(lt => lt.CompanyId == _currentUserService.CompanyId)
                .Where(lt => lt.LeaveTypeId == request.LeaveType)
                .FirstOrDefaultAsync();
            if (leaveType == null)
            {
                return new()
                {
                    Errors = ["The selected leave type could not be found"]
                };
            }

            var user = (await _dataContext.Users
                .FindById(request.Employee ?? _currentUserService.UserId)
                .Include(u => u.Team)
                .Include(u => u.Company.Schedule)
                .Include(u => u.Schedule)
                .FirstOrDefaultAsync())
                ?? throw new NotFoundException();

            if (!await CheckOverlappingBookingsAsync(request, user.UserId))
            {
                return new()
                {
                    Errors = ["The requested absence overlaps another absence"]
                };
            }

            var days = 0d;

            var absence = new Entities.Leave
            {
                DateStart = request.Start,
                DayPartStart = request.StartPart,
                DateEnd = request.End,
                DayPartEnd = request.EndPart,
                LeaveTypeId = request.LeaveType,
                Days = days,
                EmployeeComment = request.Comment,
                User = user,
                Status = LeaveStatus.New,
                ApproverId = await GetApproverAsync(user.UserId),
            };

            if (leaveType.UseAllowance)
                await _daysCalculator.CalculateDaysAsync(absence);

            NewApprovalMessage? message = null;
            if (absence.ApproverId == user.UserId)
            {
                absence.Status = LeaveStatus.Approved;
                absence.DecidedOn = DateTime.Today;
            }
            else
            {
                var count = await _dataContext.Leaves
                    .Where(a => a.ApproverId == absence.ApproverId)
                    .Where(a => a.Status == LeaveStatus.New || a.Status == LeaveStatus.PendingRevoke)
                    .CountAsync();
                message = new()
                {
                    Approver = absence.ApproverId,
                    Count = count,
                };
            }

            _dataContext.Leaves.Add(absence);
            await _dataContext.SaveChangesAsync();

            if (message != null)
            {
                await _mediator.Publish(message);
            }

            return new();
        }

        private async Task<int> GetApproverAsync(int userId)
        {
            var user = await _dataContext.Users
                 .Where(u => u.UserId == userId)
                 .Select(u => new
                 {
                     u.AutoApprove,
                     u.Team.ManagerId,
                 })
                 .FirstAsync();

            if (user.AutoApprove)
                return userId;
            else
                return user.ManagerId!.Value;
        }

        private async Task<bool> CheckOverlappingBookingsAsync(BookCommand request, int userId)
        {
            var matching = await _dataContext.Leaves
                .Where(a => a.UserId == userId)
                .Where(a =>
                    a.DateStart >= request.Start && a.DateStart <= request.End
                    || a.DateEnd >= request.Start && a.DateEnd <= request.End
                    )
                .ToArrayAsync();

            foreach (var item in matching)
            {
                if (item.DayPartStart != LeavePart.Afternoon && request.EndPart != LeavePart.Morning)
                    return false;

                if (item.DayPartEnd != LeavePart.Morning && request.StartPart != LeavePart.Afternoon)
                    return false;
            }

            return true;
        }
    }
}