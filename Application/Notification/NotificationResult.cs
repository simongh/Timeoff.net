namespace Timeoff.Application.Notification
{
    public record NotificationResult
    {
        public string Type { get; init; }

        public int Count { get; init; }
    }
}