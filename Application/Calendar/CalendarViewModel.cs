namespace Timeoff.Application.Calendar
{
    public record CalendarViewModel
    {
        public int CurrentYear { get; init; }

        public bool ShowFullYear { get; init; }

        public string Name { get; init; }

        public int NextYear => CurrentYear + 1;

        public int LastYear => CurrentYear - 1;

        public IEnumerable<ResultModels.CalendarMonthResult> Calendar { get; init; }

        public ResultModels.AllowanceSummaryResult AllowanceSummary { get; init; }
    }
}