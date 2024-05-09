namespace Timeoff.ResultModels
{
    public record DayResult
    {
        public int Id { get; init; }
        public DateTime Date { get; init; }

        public string? Name { get; init; }

        public bool IsHoliday { get; init; }

        public string? Colour { get; init; }

        public LeavePart? DayPart { get; init; }
    }
}