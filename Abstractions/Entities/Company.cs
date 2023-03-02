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

        public Guid IntegrationApiToken { get; set; }

        public int CarryOver { get; set; }

        public int CompanyId { get; private set; }

        public ICollection<Department> Departments { get; set; }

        public ICollection<User> Users { get; set; }

        public ICollection<BankHoliday> BankHolidays { get; set; }

        public ICollection<EmailAudit> EmailAudits { get; set; }

        public ICollection<Audit> Audits { get; set; }
    }
}