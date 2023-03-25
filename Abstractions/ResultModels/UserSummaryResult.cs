namespace Timeoff.ResultModels
{
    public record UserSummaryResult : Types.UserModel
    {
        public IEnumerable<(int Id, double Total)> LeaveSummary { get; init; }

        public double AllowanceUsed { get; init; }
    }
}