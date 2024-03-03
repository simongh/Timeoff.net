namespace Timeoff.Application.EmailAudit
{
    public record QueryResult
    {
        public IEnumerable<EmailResult> Results { get; init; } = null!;

        public int Pages { get; init; }
    }
}