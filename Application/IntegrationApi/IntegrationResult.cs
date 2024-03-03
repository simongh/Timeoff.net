namespace Timeoff.Application.IntegrationApi
{
    public record IntegrationResult
    {
        public bool Enabled { get; init; }

        public string ApiKey { get; init; } = null!;
    }
}