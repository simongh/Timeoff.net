namespace Timeoff.Application.Settings
{
    public record LeaveTypeResult
    {
        public int Id { get; init; }

        public string Name { get; init; } = null!;

        public int Limit { get; init; }

        public string Colour { get; init; } = null!;

        public bool UseAllowance { get; init; }

        public bool AutoApprove { get; init; }

        public bool First { get; init; }
    }
}