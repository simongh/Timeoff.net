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

        public int UserId { get; private set; }

        public int CompanyId { get; set; }

        public Company Company { get; set; } = null!;

        public int DepartmentId { get; set; }

        public Team Department { get; set; } = null!;

        public Schedule? Schedule { get; set; }

        public ICollection<Team> ManagedDepartments { get; set; } = new HashSet<Team>();

        public ICollection<Team> DepartmentsSupervised { get; set; } = new HashSet<Team>();

        public ICollection<Leave> Leave { get; set; } = new HashSet<Leave>();

        public ICollection<UserFeed> Feeds { get; set; } = new HashSet<UserFeed>();

        public ICollection<UserAllowanceAdjustment> Adjustments { get; set; } = new HashSet<UserAllowanceAdjustment>();
    }
}