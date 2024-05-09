namespace Timeoff.Entities
{
    public class Team
    {
        public required string Name { get; set; }

        public double Allowance { get; set; } = 20;

        public bool IncludePublicHolidays { get; set; } = true;

        public bool IsAccrued { get; set; }

        public int TeamId { get; init; }

        public int? ManagerId { get; set; }

        public User? Manager { get; set; }

        public int CompanyId { get; init; }

        public Company? Company { get; init; }

        public byte[]? RowVersion { get; init; }
        public ICollection<User> Users { get; set; } = [];

        public ICollection<User> Approvers { get; set; } = [];
    }
}