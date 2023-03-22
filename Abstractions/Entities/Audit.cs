namespace Timeoff.Entities
{
    public class Audit
    {
        public string EntityType { get; set; }

        public int EntityId { get; set; }

        public string Attribute { get; set; }

        public string? OldValue { get; set; }

        public string? NewValue { get; set; }

        public DateTime CreatedAt { get; set; }

        public int AuditId { get; private set; }

        public int CompanyId { get; set; }

        public Company Company { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }
    }
}