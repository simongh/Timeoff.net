namespace Timeoff.ResultModels
{
    public record UserSummaryResult
    {
        public int Id { get; init; }

        public string Name { get; init; }

        public IEnumerable<(int Id, double Total)> LeaveSummary { get; init; }

        public double AllowanceUsed { get; init; }
    }
}