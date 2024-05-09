namespace Timeoff.Entities
{
    public class EmailAudit
    {
        public required string Email { get; init; }

        public required string Subject { get; init; }

        public required string Body { get; init; }

        public DateTime CreatedAt { get; init; }

        public int EmailAuditId { get; init; }

        public int CompanyId { get; init; }

        public Company? Company { get; init; }

        public int UserId { get; init; }

        public required User User { get; init; }
    }
}