namespace Timeoff.ResultModels
{
    public class CalendarDayResult
    {
        public int UserId { get; init; }

        public DateTime? Date { get; init; }

        public string Day => Date?.Day.ToString() ?? "";

        public bool IsPublicHoliday => HolidayName != null;

        public bool IsWeekend => Date?.DayOfWeek == DayOfWeek.Sunday || Date?.DayOfWeek == DayOfWeek.Saturday;

        public bool IsLeave => LeaveId.HasValue;

        public bool IsMorning { get; init; }

        public bool IsAfternoon { get; init; }

        public bool IsToday => Date == DateTime.Today;

        public bool IsPending => LeaveStatus.HasValue && LeaveStatus.Value == Timeoff.LeaveStatus.New;

        public string MorningColourClass { get; init; } = "leave_type_color_1";
        public string AfternoonColourClass { get; init; } = "leave_type_color_1";

        public LeaveStatus? LeaveStatus { get; init; }

        public string? HolidayName { get; init; }

        public int? LeaveId { get; init; }

        public string? LeaveMessage { get; init; }
    }
}