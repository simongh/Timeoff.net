namespace Timeoff.Entities
{
    public class Company
    {
        public string Name { get; set; }

        public string Country { get; set; }

        public byte StartOfNewYear { get; set; }

        public bool ShareAllAbsences { get; set; }

        public bool IsTeamViewHidden { get; set; }

        public bool LdapAuthEnabled { get; set; }

        public string? LdapAuthConfig { get; set; }

        public string DateFormat { get; set; } = "yyyy-MM-dd";

        public string? CompanyWideMessage { get; set; }

        public byte Mode { get; set; }

        public string TimeZone { get; set; } = "Europe/London";

        public bool IntegrationApiEnabled { get; set; }

        public Guid IntegrationApiToken { get; set; } = Guid.NewGuid();

        public int CarryOver { get; set; }

        public int CompanyId { get; private set; }

        public byte[]? RowVersion { get; init; } = null!;

        public Schedule Schedule { get; set; } = null!;

        public ICollection<Team> Teams { get; set; } = new HashSet<Team>();

        public ICollection<User> Users { get; set; } = new HashSet<User>();

        public ICollection<PublicHoliday> PublicHolidays { get; set; } = new HashSet<PublicHoliday>();

        public ICollection<EmailAudit> EmailAudits { get; set; } = new HashSet<EmailAudit>();

        //public ICollection<Audit> Audits { get; set; } = new HashSet<Audit>();

        public ICollection<LeaveType> LeaveTypes { get; set; } = new HashSet<LeaveType>();
    }
}