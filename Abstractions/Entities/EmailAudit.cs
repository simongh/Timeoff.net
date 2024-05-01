namespace Timeoff.Entities
{
    public class EmailAudit
    {
        public string Email { get; init; } = null!;

        public string Subject { get; init; } = null!;

        public string Body { get; init; } = null!;

        public DateTime CreatedAt { get; init; }

        public int EmailAuditId { get; init; }

        public int CompanyId { get; init; }

        public Company Company { get; init; } = null!;

        public int UserId { get; init; }

        public User User { get; init; } = null!;
    }
}