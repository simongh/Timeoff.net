namespace Timeoff.ResultModels
{
    public record LeaveSummaryResult
    {
        public double Total { get; init; }

        public bool AffectsAllowance { get; init; }

        public string Name { get; init; } = null!;

        public double Allowance { get; init; }
    }
}