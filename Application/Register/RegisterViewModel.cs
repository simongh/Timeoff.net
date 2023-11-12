namespace Timeoff.Application.Register
{
    public record RegisterViewModel : RegisterModel
    {
        public IEnumerable<ResultModels.CountryResult> Countries { get; init; } = null!;

        public IEnumerable<(string Id, string Name)> TimeZones { get; init; } = null!;

        public bool Success { get; init; }

        public ResultModels.FlashResult Result { get; init; } = new();
    }
}