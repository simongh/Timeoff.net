namespace Timeoff.ResultModels
{
    public record LeaveRequestedResult
    {
        public DateTime StartDate { get; init; }

        public LeavePart StartPart { get; init; }

        public DateTime EndDate { get; init; }

        public LeavePart EndPart { get; init; }

        public string DateFormat { get; init; } = null!;

        public int Id { get; init; }

        public ListResult Approver { get; init; } = null!;

        public LeaveTypeResult Type { get; init; } = null!;

        public double Days { get; init; }

        public LeaveStatus Status { get; init; }

        public string? Comment { get; init; }
    }
}