namespace Timeoff.ResultModels
{
    public record FlashResult
    {
        public IEnumerable<string>? Errors { get; init; }

        public IEnumerable<string>? Messages { get; init; }
    }
}