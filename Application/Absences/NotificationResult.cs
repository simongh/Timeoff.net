namespace Timeoff.Application.Absences
{
    public record NotificationResult
    {
        public string Type { get; init; }

        public int Count { get; init; }
    }
}