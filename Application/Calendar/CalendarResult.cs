namespace Timeoff.Application.Calendar
{
    public record CalendarResult
    {
        public string FirstName { get; init; } = null!;

        public string LastName { get; init; } = null!;

        public bool IsActive { get; init; }

        public ResultModels.AllowanceSummaryResult Summary { get; init; } = null!;

        public IEnumerable<ResultModels.PublicHolidayResult> Holidays { get; init; } = null!;
        public IEnumerable<ResultModels.LeaveRequestedResult> LeaveRequested { get; init; } = null!;
    }
}