namespace Timeoff.ResultModels
{
    public record AllowanceSummaryResult : Types.AllowanceCalculator
    {
        public IEnumerable<LeaveSummaryResult> LeaveSummary { get; init; } = null!;

        public double Used => LeaveSummary.Where(l => l.AffectsAllowance).Sum(l => l.Total);

        public double Remaining => Total - Used;

        public double Available
        {
            get
            {
                if (Start.Year > Year || End?.Year < Year)
                {
                    return 0;
                }

                return Total - Used;
            }
        }
    }
}