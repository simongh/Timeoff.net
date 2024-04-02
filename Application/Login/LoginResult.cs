namespace Timeoff.Application.Login
{
    public record LoginResult
    {
        public string? CompanyName { get; init; }

        public bool ShowTeamView { get; init; }

        public string? DateFormat { get; init; }

        public string? Name { get; init; }

        public string? Token { get; init; }

        public DateTimeOffset Expires { get; init; }

        public bool IsAdmin { get; init; }

        public bool Success { get; init; }
    }
}