namespace Timeoff.Entities
{
    public class Team
    {
        public string Name { get; set; } = null!;

        public double Allowance { get; set; } = 20;

        public bool IncludePublicHolidays { get; set; } = true;

        public bool IsAccrued { get; set; }

        public int TeamId { get; private set; }

        public int? ManagerId { get; set; }

        public User? Manager { get; set; }

        public int CompanyId { get; set; }

        public Company Company { get; set; } = null!;

        public ICollection<User> Users { get; set; } = new HashSet<User>();

        public ICollection<User> Approvers { get; set; } = new HashSet<User>();
    }
}