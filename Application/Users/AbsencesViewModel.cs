namespace Timeoff.Application.Users
{
    public record AbsencesViewModel : Types.UserModel
    {
        public ResultModels.AllowanceSummaryResult Summary { get; init; } = null!;

        public int TeamId { get; init; }

        public double UsedPercent => (Summary.Used / Summary.TotalAllowance) * 100;

        public double RemainingPercent => (Summary.Remaining / Summary.TotalAllowance) * 100;
    }
}