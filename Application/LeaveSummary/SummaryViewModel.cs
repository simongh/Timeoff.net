namespace Timeoff.Application.LeaveSummary
{
    public record SummaryViewModel
    {
        public bool ShowDetailed { get; init; }

        public DateTime StartDate { get; init; }

        public DateTime EndDate { get; init; }

        public string DateFormat { get; init; } = null!;

        public string Name { get; init; } = null!;

        public double Days { get; init; }

        public string Approver { get; init; } = null!;

        public DateTime Created { get; init; }

        public string Type { get; init; } = null!;

        public LeaveStatus Status { get; init; }

        public string? Comment { get; init; }
    }
}