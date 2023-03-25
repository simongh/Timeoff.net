namespace Timeoff.ResultModels
{
    public record PublicHolidaysViewModel
    {
        public int CurrentYear { get; init; }

        public int NextYear => CurrentYear + 1;

        public int PreviousYear => CurrentYear - 1;

        public string CompanyName { get; init; }

        public string DateFormat { get; init; }

        public IEnumerable<CalendarMonthResult> Calendar { get; init; }

        public PublicHolidayResult[] BankHolidays { get; init; }

        public FlashResult Result { get; set; } = new();
    }
}