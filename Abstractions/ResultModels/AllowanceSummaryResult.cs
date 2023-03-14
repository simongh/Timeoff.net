namespace Timeoff.ResultModels
{
    public class AllowanceSummaryResult
    {
        public double Used { get; init; }

        public double TotalAllowance { get; init; }

        public double Remaining => TotalAllowance - Used;
    }
}