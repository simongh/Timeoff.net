namespace Timeoff.ResultModels
{
    public record IntegrationApiViewModel
    {
        public bool Enabled { get; init; }

        public string ApiKey { get; init; } = null!;

        public string Name { get; init; } = null!;

        public FlashResult? Messages { get; set; }
    }
}