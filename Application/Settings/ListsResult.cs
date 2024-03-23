namespace Timeoff.Application.Settings
{
    public record ListsResult
    {
        public IEnumerable<ResultModels.CountryResult> Countries { get; init; } = null!;

        public IEnumerable<dynamic> TimeZones { get; init; } = null!;
    }
}