namespace Timeoff.ResultModels
{
    public record LeaveTypeResult
    {
        public int Id { get; init; }

        public string Name { get; init; }

        public int Limit { get; init; }

        public string Colour { get; init; }

        public bool UseAllowance { get; init; }

        public bool AutoApprove { get; init; }

        public bool First { get; init; }
    }
}