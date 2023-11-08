namespace Timeoff.ResultModels
{
    public record AllowanceSummaryResult
    {
        public int PreviousYear { get; init; }

        public double Allowance { get; init; }

        public double CarryOver { get; init; }

        public double Adjustment { get; init; }

        public double EmploymentAdjustment { get; init; }

        public bool IsAccrued { get; init; }

        public double AccruedAdjustment { get; init; }

        public IEnumerable<LeaveSummaryResult> LeaveSummary { get; init; } = null!;

        public double Used => LeaveSummary.Where(l => l.AffectsAllowance).Sum(l => l.Total);

        public double Remaining => Total - Used;
        public double Total => Allowance + CarryOver + Adjustment + EmploymentAdjustment + AccruedAdjustment;

        public double Available => Total - Used;
    }
}