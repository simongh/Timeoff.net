namespace Timeoff.Application.Absences
{
    public record NotificationsSummaryResult
    {
        public IEnumerable<NotificationResult> Data { get; init; }
    }
}