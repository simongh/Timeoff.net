namespace Timeoff.ResultModels
{
    public class CalendarDayResult
    {
        public string Day { get; init; }

        public bool IsBankHoliday { get; init; }

        public bool IsWeekend { get; init; }

        public bool IsLeave { get; init; }

        public bool IsMorning { get; init; }

        public bool IsAfternoon { get; init; }

        public bool IsToday { get; init; }

        public bool IsPending { get; init; }
    }
}