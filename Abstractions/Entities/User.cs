namespace Timeoff.Entities
{
    public class User
    {
        public required string Email { get; set; }

        public required string Password { get; set; }

        public string? Token { get; set; }

        public required string FirstName { get; set; }

        public required string LastName { get; set; }

        public bool IsActivated { get; set; }

        public bool IsAdmin { get; set; }

        public bool AutoApprove { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string Fullname => $"{FirstName} {LastName}";

        public bool IsActive => EndDate == null || DateTime.Today > EndDate;

        public int UserId { get; init; }

        public int CompanyId { get; init; }

        public Company? Company { get; init; }

        public int TeamId { get; set; }

        public Team? Team { get; set; }

        public Schedule? Schedule { get; set; }

        public byte[]? RowVersion { get; init; } = null!;

        public ICollection<Team> ManagedTeams { get; set; } = [];

        public ICollection<Team> TeamApprover { get; set; } = [];

        public ICollection<Leave> Leave { get; set; } = [];

        public ICollection<UserFeed> Feeds { get; set; } = [];

        public ICollection<UserAllowanceAdjustment> Adjustments { get; set; } = [];

        public ICollection<Calendar> Calendar { get; set; } = [];
    }
}