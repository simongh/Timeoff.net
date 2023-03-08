namespace Timeoff.Entities
{
    public class User
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public string Name { get; set; }

        public string LastName { get; set; }

        public bool Activated { get; set; }

        public bool Admin { get; set; }

        public bool AutoApprove { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string Fullname => $"{Name} {LastName}";

        public bool IsActive => EndDate == null || DateTime.Today > EndDate;

        public int UserId { get; private set; }

        public int CompanyId { get; set; }

        public Company Company { get; set; }

        public int DepartmentId { get; set; }

        public Department Department { get; set; }

        public Schedule? Schedule { get; set; }

        public ICollection<Department> ManagedDepartments { get; set; } = new HashSet<Department>();

        public ICollection<Leave> Leave { get; set; } = new HashSet<Leave>();

        public ICollection<UserFeed> Feeds { get; set; } = new HashSet<UserFeed>();

        public ICollection<UserAllowanceAdjustment> Adjustments { get; set; } = new HashSet<UserAllowanceAdjustment>();
    }
}