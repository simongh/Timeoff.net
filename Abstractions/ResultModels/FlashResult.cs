namespace Timeoff.ResultModels
{
    public record FlashResult
    {
        public IEnumerable<string>? Errors { get; init; }

        public IEnumerable<string>? Messages { get; init; }

        public static FlashResult WithError(string error)
        {
            return new()
            {
                Errors = new[] { error },
            };
        }

        public static FlashResult Success(string message)
        {
            return new()
            {
                Messages = new[] { message },
            };
        }
    }
}