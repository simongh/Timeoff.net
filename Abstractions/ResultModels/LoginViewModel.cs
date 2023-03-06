namespace Timeoff.ResultModels
{
    public record LoginViewModel
    {
        public bool AllowRegistrations { get; init; }

        public FlashResult? Result { get; init; }

        public bool Success { get; init; }
    }
}