namespace Timeoff.ResultModels
{
    public record TokenResult
    {
        public string? CompanyName { get; init; }

        public bool ShowTeamView { get; init; }

        public string? DateFormat { get; init; }

        public string? Name { get; init; }

        public bool IsAdmin { get; init; }

        public bool Success { get; init; }

        public string Token { get; init; } = null!;

        public DateTimeOffset Expires { get; init; }
    }
}