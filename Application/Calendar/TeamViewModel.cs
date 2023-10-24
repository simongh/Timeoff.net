using Timeoff.ResultModels;

namespace Timeoff.Application.Calendar
{
    public record TeamViewModel
    {
        public string Name { get; init; } = null!;

        public DateTime CurrentDate { get; init; }

        public DateTime Previous => CurrentDate.AddMonths(-1);

        public DateTime Next => CurrentDate.AddMonths(1);

        public bool Grouped { get; init; }

        public IEnumerable<ListItem> Teams { get; init; } = null!;

        public int? SelectedTeam { get; init; }

        public IEnumerable<UserResult> Users { get; init; } = null!;

        public IEnumerable<CalendarDayResult> CalendarDays { get; init; } = null!;

        public int DaysDeducted(int userId)
        {
            return (int)CalendarDays
                .Where(d => d.UserId == userId && d.IsLeave && d.LeaveStatus == LeaveStatus.Approved)
                .Sum(d => d.IsAfternoon || d.IsMorning ? 0.5 : 1);
        }
    }
}