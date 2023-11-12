namespace Timeoff.Application.Notification
{
    public record NotificationsSummaryResult
    {
        public IEnumerable<NotificationResult> Data { get; init; }
    }
}