namespace Timeoff.Application.Login
{
    public record LoginResult : ResultModels.TokenResult
    {
        public string? CompanyName { get; init; }

        public bool ShowTeamView { get; init; }

        public string? DateFormat { get; init; }

        public string? Name { get; init; }

        public bool IsAdmin { get; init; }

        public bool Success { get; init; }
    }
}