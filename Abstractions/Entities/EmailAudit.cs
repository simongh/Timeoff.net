namespace Timeoff.Entities
{
    public class EmailAudit
    {
        public string Email { get; set; } = null!;

        public string Subject { get; set; } = null!;

        public string Body { get; set; } = null!;

        public DateTime CreatedAt { get; set; }

        public int EmailAuditId { get; private set; }

        public int CompanyId { get; set; }

        public Company Company { get; set; } = null!;

        public int UserId { get; set; }

        public User User { get; set; } = null!;
    }
}