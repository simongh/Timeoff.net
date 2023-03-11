namespace Timeoff.ResultModels
{
    public record CalendarMonthResult
    {
        public DateTime Date { get; init; }

        public IEnumerable<CalendarDayResult> Days { get; init; }

        public static CalendarMonthResult FromDate(DateTime date)
        {
            var weeks = new List<CalendarDayResult>();
            var offset = ((int)date.DayOfWeek + 6) % 7;
            var end = date.AddMonths(1);

            var day = date.AddDays(-offset);
            while (day < end)
            {
                if (day.Ticks < date.Ticks)
                    weeks.Add(new()
                    {
                        Day = ""
                    });
                else
                    weeks.Add(new()
                    {
                        Day = day.Day.ToString(),
                        IsWeekend = day.DayOfWeek == DayOfWeek.Sunday || day.DayOfWeek == DayOfWeek.Saturday,
                        IsToday = day == DateTime.Today,
                    });
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