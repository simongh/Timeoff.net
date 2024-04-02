namespace Timeoff.Application.Users
{
    public record UserInfoResult : Types.UserModel
    {
        public ResultModels.ListResult Team { get; init; } = null!;

        public bool IsAdmin { get; init; }

        public double AvailableAllowance => AllowanceCalculator.Total;

        public double DaysUsed => AllowanceCalculator.DaysUsed;

        internal Types.AllowanceCalculator AllowanceCalculator { get; init; } = null!;
    }
}