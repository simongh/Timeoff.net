namespace Timeoff.Entities
{
    public class Company
    {
        public string Name { get; set; } = null!;

        public string Country { get; set; } = null!;

        public byte StartOfNewYear { get; set; }

        public bool ShareAllAbsences { get; set; }

        public bool IsTeamViewHidden { get; set; }

        public bool LdapAuthEnabled { get; set; }

        public string? LdapAuthConfig { get; set; }

        public string DateFormat { get; set; } = "yyyy-mm-dd";

        public string TimeZone { get; set; } = TimeZoneInfo.Local.DisplayName;

        public bool IntegrationApiEnabled { get; set; }

        public Guid IntegrationApiToken { get; set; } = Guid.NewGuid();

        public int CarryOver { get; set; }

        public int CompanyId { get; init; }

        public byte[]? RowVersion { get; init; } = null!;

        public Schedule Schedule { get; set; } = null!;

        public ICollection<Team> Teams { get; set; } = [];

        public ICollection<User> Users { get; set; } = [];

        public ICollection<EmailAudit> EmailAudits { get; set; } = [];

        public ICollection<LeaveType> LeaveTypes { get; set; } = [];

        public ICollection<Calendar> Calendar { get; set; } = [];
    }
}