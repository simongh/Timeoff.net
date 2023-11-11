namespace Timeoff.ResultModels
{
    public record CalendarResult
    {
        public DateTime StartDate { get; init; }

        public int Months { get; init; }

        public IEnumerable<PublicHolidayResult> Holidays { get; init; } = null!;

        public DateTime CurrentMonth(int offset) => StartDate.AddMonths(offset);

        public IEnumerable<CalendarDayResult> GetWeeks(
            int month,
            IEnumerable<LeaveRequestedResult>? absences = null)
        {
            absences ??= Array.Empty<LeaveRequestedResult>();

            var date = CurrentMonth(month);
            var offset = ((int)date.DayOfWeek + 6) % 7;
            var end = date.AddMonths(1);

            var day = date.AddDays(-offset);
            while (day < end)
            {
                if (day.Ticks < date.Ticks)
                    yield return new()
                    { };
                else
                {
                    var leave = absences.FirstOrDefault(a => a.StartDate >= day && a.EndDate <= day);
                    var holiday = Holidays.FirstOrDefault(h => h.Date == day);

                    yield return new()
                    {
                        Date = day,
                        HolidayName = holiday?.Name,
                        LeaveMessage = leave?.Comment,
                        LeaveStatus = leave?.Status,
                        IsMorning = leave != null && leave.StartPart != LeavePart.Afternoon,
                        IsAfternoon = leave != null && leave.EndPart != LeavePart.Morning,
                        LeaveId = leave?.Id,
                    };
                }
                day = day.AddDays(1);
            }
        }
    }
}