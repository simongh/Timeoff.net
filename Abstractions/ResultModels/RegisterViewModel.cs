namespace Timeoff.ResultModels
{
    public record RegisterViewModel : Types.RegisterModel
    {
        public IEnumerable<CountryResult> Countries { get; init; }

        public IEnumerable<(string Id, string Name)> TimeZones { get; init; }

        public bool Success { get; init; }

        public FlashResult Result { get; init; } = new();
    }
}