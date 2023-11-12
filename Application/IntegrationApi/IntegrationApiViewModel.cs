namespace Timeoff.Application.IntegrationApi
{
    public record IntegrationApiViewModel
    {
        public bool Enabled { get; init; }

        public string ApiKey { get; init; } = null!;

        public string Name { get; init; } = null!;

        public ResultModels.FlashResult? Messages { get; set; }
    }
}