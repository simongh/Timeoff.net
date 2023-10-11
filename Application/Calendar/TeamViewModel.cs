namespace Timeoff.Application.Calendar
{
    public record TeamViewModel
    {
        public string Name { get; init; }

        public DateTime CurrentDate { get; init; }

        public DateTime Previous => CurrentDate.AddMonths(-1);

        public DateTime Next => CurrentDate.AddMonths(1);

        public bool Grouped { get; init; }
    }
}