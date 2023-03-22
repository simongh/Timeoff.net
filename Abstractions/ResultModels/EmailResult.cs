namespace Timeoff.ResultModels
{
    public record EmailResult
    {
        public int Id { get; init; }

        public int UserId { get; init; }

        public string Name { get; init; } = null!;

        public string Email { get; init; } = null!;

        public string Subject { get; init; } = null!;

        public string Body { get; init; } = null!;

        public DateTimeOffset CreatedAt { get; init; }
    }
}