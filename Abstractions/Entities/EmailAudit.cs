namespace Timeoff.Entities
{
    public class EmailAudit
    {
        public string Email { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }

        public DateTime CreatedAt { get; set; }

        public int EmailAuditId { get; private set; }

        public int CompanyId { get; set; }

        public Company Company { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }
    }
}