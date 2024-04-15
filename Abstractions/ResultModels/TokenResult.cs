namespace Timeoff.ResultModels
{
    public record TokenResult
    {
        public string Token { get; init; } = null!;

        public DateTimeOffset Expires { get; init; }
    }
}