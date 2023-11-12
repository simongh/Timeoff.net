using Timeoff.ResultModels;

namespace Timeoff.Application.Calendar
{
    public record UserCalendarViewModel : Types.UserModel
    {
        public int CurrentYear { get; init; }

        public int NextYear => CurrentYear + 1;

        public int PreviousYear => CurrentYear - 1;

        public AllowanceSummaryResult Summary { get; init; } = null!;

        public CalendarResult Calendar { get; init; } = null!;

        public IEnumerable<LeaveRequestedResult> LeaveRequested { get; init; } = null!;
    }
}