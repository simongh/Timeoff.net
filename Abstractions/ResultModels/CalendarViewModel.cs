namespace Timeoff.ResultModels
{
    public record CalendarViewModel
    {
        public int CurrentYear { get; init; }

        public bool ShowFullYear { get; init; }

        public string Name { get; init; }

        public int NextYear => CurrentYear + 1;

        public int LastYear => CurrentYear - 1;

        public IEnumerable<CalendarMonthResult> Calendar { get; init; }

        public AllowanceSummaryResult AllowanceSummary { get; init; }
    }
}