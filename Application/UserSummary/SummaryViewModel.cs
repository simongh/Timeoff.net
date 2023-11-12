namespace Timeoff.Application.UserSummary
{
    public record SummaryViewModel
    {
        public string Name { get; init; } = null!;

        public string Team { get; init; } = null!;

        public double Total { get; init; }

        public double Used { get; init; }

        public string Approver { get; init; } = null!;

        public bool ShowDetailed { get; init; }
    }
}