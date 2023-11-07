namespace Timeoff.Application.Users
{
    public record CalendarViewModel : Types.UserModel
    {
        public int CurrentYear { get; init; }

        public int NextYear => CurrentYear + 1;

        public int PreviousYear => CurrentYear - 1;

        public ResultModels.AllowanceSummaryResult Summary { get; init; } = null!;

        public IEnumerable<ResultModels.CalendarMonthResult> Calendar { get; init; } = null!;
    }
}