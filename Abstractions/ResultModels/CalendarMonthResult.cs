namespace Timeoff.ResultModels
{
    public record CalendarMonthResult
    {
        public DateTime Date { get; init; }

        public IEnumerable<CalendarDayResult> Days { get; init; }

        public static CalendarMonthResult FromDate(
            DateTime date,
            IEnumerable<Entities.Leave> absences,
            IEnumerable<PublicHolidayResult> holidays)
        {
            var weeks = new List<CalendarDayResult>();
            var offset = ((int)date.DayOfWeek + 6) % 7;
            var end = date.AddMonths(1);

            var day = date.AddDays(-offset);
            while (day < end)
            {
                if (day.Ticks < date.Ticks)
                    weeks.Add(new()
                    { });
                else
                {
                    var leave = absences.FirstOrDefault(a => a.DateStart >= day && a.DateEnd <= day);
                    var holiday = holidays.FirstOrDefault(h => h.Date == day);

                    weeks.Add(new()
                    {
                        Date = day,
                        HolidayName = holiday?.Name,
                        LeaveMessage = leave?.EmployeeComment,
                        LeaveStatus = leave?.Status,
                        IsMorning = leave?.DayPartStart == LeavePart.Morning,
                        IsAfternoon = leave?.DayPartEnd == LeavePart.Afternoon,
                        LeaveId = leave?.LeaveId,
                    });
                }
                day = day.AddDays(1);
            }

            return new()
            {
                Date = date,
                Days = weeks,
            };
        }
    }
}