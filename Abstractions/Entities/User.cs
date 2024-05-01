namespace Timeoff.Entities
{
    public class User
    {
        public string Email { get; set; } = null!;

        public string Password { get; set; } = null!;

        public string? Token { get; set; }

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public bool IsActivated { get; set; }

        public bool IsAdmin { get; set; }

        public bool AutoApprove { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string Fullname => $"{FirstName} {LastName}";

        public bool IsActive => EndDate == null || DateTime.Today > EndDate;

        public int UserId { get; init; }

        public int CompanyId { get; init; }

        public Company Company { get; init; } = null!;

        public int TeamId { get; set; }

        public Team Team { get; set; } = null!;

        public Schedule? Schedule { get; set; }

        public byte[]? RowVersion { get; init; } = null!;

        public ICollection<Team> ManagedTeams { get; set; } = new HashSet<Team>();

        public ICollection<Team> TeamApprover { get; set; } = new HashSet<Team>();

        public ICollection<Leave> Leave { get; set; } = new HashSet<Leave>();

        public ICollection<UserFeed> Feeds { get; set; } = new HashSet<UserFeed>();

        public ICollection<UserAllowanceAdjustment> Adjustments { get; set; } = new HashSet<UserAllowanceAdjustment>();
    }
}