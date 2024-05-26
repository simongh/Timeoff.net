namespace Timeoff.Application.TeamView
{
    public record TeamResult
    {
        public int Id { get; init; }

        public required string Name { get; init; }

        public required ResultModels.ListResult User { get; init; }

        public required IEnumerable<ResultModels.DayResult> Days { get; init; }

        public double Used { get; init; }
    }
}