using MediatR;

namespace Timeoff.Application.BookAbsence
{
    public record NewApprovalMessage : INotification
    {
        public int Approver { get; init; }

        public int Count { get; init; }
    }
}