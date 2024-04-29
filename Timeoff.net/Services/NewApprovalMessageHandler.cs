using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace Timeoff.Services
{
    internal class NewApprovalMessageHandler(IHubContext<RequestsHub, IRequestsHub> hubContext)
        : INotificationHandler<Application.BookAbsence.NewApprovalMessage>
    {
        private readonly IHubContext<RequestsHub, IRequestsHub> _hubContext = hubContext;

        public async Task Handle(Application.BookAbsence.NewApprovalMessage notification, CancellationToken cancellationToken)
        {
            await _hubContext
                .Clients
                .User(notification.Approver.ToString())
                .AwaitingApproval(notification.Count);
        }
    }
}