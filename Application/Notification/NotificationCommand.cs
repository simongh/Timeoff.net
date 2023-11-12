using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Timeoff.Application.Notification
{
    public record NotificationCommand : IRequest<NotificationsSummaryResult>
    {
    }

    internal class NotificationCommandHandler : IRequestHandler<NotificationCommand, NotificationsSummaryResult>
    {
        private readonly IDataContext _dataContext;
        private readonly Services.ICurrentUserService _userService;

        public NotificationCommandHandler(
            IDataContext dataContext,
            Services.ICurrentUserService userService)
        {
            _dataContext = dataContext;
            _userService = userService;
        }

        public async Task<NotificationsSummaryResult> Handle(NotificationCommand request, CancellationToken cancellationToken)
        {
            var count = await _dataContext.Leaves
                .Where(a => a.ApproverId == _userService.UserId)
                .Where(a => a.Status == LeaveStatus.New || a.Status == LeaveStatus.PendingRevoke)
                .CountAsync();

            return new()
            {
                Data = new List<NotificationResult>
                {
                    new NotificationResult
                    {
                        Type = "pending_request",
                        Count = count,
                    },
                },
            };
        }
    }
}