namespace Timeoff.Application.AllowanceUsage
{
    public record UserSummaryResult : Types.UserModel
    {
        public IEnumerable<LeaveSummaryResult> LeaveSummary { get; init; } = null!;

        public double AllowanceUsed { get; init; }
    }
}