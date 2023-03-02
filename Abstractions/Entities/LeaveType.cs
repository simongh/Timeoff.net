namespace Timeoff.Entities
{
    public class LeaveType
    {
        public string Name { get; set; }

        public string Colour { get; set; } = "#ffffff";

        public bool UseAllowance { get; set; } = true;

        public int Limit { get; set; } = 0;

        public int SortOrder { get; set; }

        public bool AutoApprove { get; set; }

        public int LeaveTypeId { get; private set; }

        public int CompanyId { get; set; }

        public Company Company { get; set; }
        public ICollection<object> Leaves { get; set; }
    }
}