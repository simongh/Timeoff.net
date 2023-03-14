namespace Timeoff.ResultModels
{
    public class CalendarDayResult
    {
        public string Day { get; init; }

        public bool IsBankHoliday => HolidayName != null;

        public bool IsWeekend { get; init; }

        public bool IsLeave => LeaveId.HasValue;

        public bool IsMorning { get; init; }

        public bool IsAfternoon { get; init; }

        public bool IsToday { get; init; }

        public bool IsPending => LeaveStatus.HasValue && LeaveStatus.Value == Timeoff.LeaveStatus.New;

        public string MorningColourClass { get; init; } = "leave_type_color_1";
        public string AfternoonColourClass { get; init; } = "leave_type_color_1";

        public LeaveStatus? LeaveStatus { get; init; }

        public string? HolidayName { get; init; }

        public int? LeaveId { get; init; }

        public string? LeaveMessage { get; init; }
    }
}