namespace Timeoff.Application.AllowanceUsage
{
    public record LeaveSummaryResult
    {
        public int Id { get; init; }

        public double AllowanceUsed { get; init; }
    }
}