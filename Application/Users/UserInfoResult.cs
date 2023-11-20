namespace Timeoff.Application.Users
{
    public record UserInfoResult : Types.UserModel
    {
        public int TeamId { get; init; }

        public string TeamName { get; init; } = null!;

        public bool IsAdmin { get; init; }

        public double AvailableAllowance => AllowanceCalculator.Total;

        public double DaysUsed => AllowanceCalculator.DaysUsed;

        public Types.AllowanceCalculator AllowanceCalculator { get; init; }
    }
}