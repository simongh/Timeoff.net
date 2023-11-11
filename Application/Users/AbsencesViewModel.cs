namespace Timeoff.Application.Users
{
    public record AbsencesViewModel : Types.UserModel
    {
        public ResultModels.AllowanceSummaryResult Summary { get; init; } = null!;

        public int TeamId { get; init; }

        public bool IsAccrued { get; init; }

        public double UsedPercent => (Summary.Used / Summary.Total) * 100;

        public double RemainingPercent => (Summary.Remaining / Summary.Total) * 100;

        public ResultModels.FlashResult? Messages { get; set; }

        public IEnumerable<ResultModels.LeaveRequestedResult> LeaveRequested { get; set; } = null!;
    }
}