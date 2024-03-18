namespace Timeoff.Application.Calendar
{
    public record UserCalendarViewModel : Types.UserModel
    {
        public int CurrentYear { get; init; }

        public int NextYear => CurrentYear + 1;

        public int PreviousYear => CurrentYear - 1;

        public ResultModels.AllowanceSummaryResult Summary { get; init; } = null!;

        public ResultModels.CalendarResult Calendar { get; init; } = null!;

        public IEnumerable<ResultModels.PublicHolidayResult> Holidays => Calendar.Holidays;

        public IEnumerable<ResultModels.LeaveRequestedResult> LeaveRequested { get; init; } = null!;
    }
}