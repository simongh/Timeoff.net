namespace Timeoff.ResultModels
{
    public record FlashResult
    {
        public IEnumerable<string>? Errors { get; init; }

        public IEnumerable<string>? Messages { get; init; }

        public Guid Result { get; set; }

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

        public static FlashResult operator +(FlashResult? a, FlashResult? b)
        {
            return new()
            {
                Errors = (a?.Errors ?? Enumerable.Empty<string>()).Concat(b?.Errors ?? Enumerable.Empty<string>()),
                Messages = (a?.Messages ?? Enumerable.Empty<string>()).Concat(b?.Messages ?? Enumerable.Empty<string>()),
            };
        }
    }
}