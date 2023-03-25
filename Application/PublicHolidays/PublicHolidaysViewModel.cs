namespace Timeoff.Application.PublicHoliday
{
    public record PublicHolidaysViewModel
    {
        public int CurrentYear { get; init; }

        public int NextYear => CurrentYear + 1;

        public int PreviousYear => CurrentYear - 1;

        public string CompanyName { get; init; } = null!;

        public string DateFormat { get; init; } = null!;

        public IEnumerable<ResultModels.CalendarMonthResult> Calendar { get; init; } = null!;

        public ResultModels.PublicHolidayResult[] PublicHolidays { get; init; } = null!;

        public ResultModels.FlashResult Result { get; set; } = new();
    }
}