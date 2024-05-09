namespace Timeoff.Entities
{
    public class LeaveType
    {
        public required string Name { get; set; }

        public string Colour { get; set; } = "#ffffff";

        public bool UseAllowance { get; set; } = true;

        public int Limit { get; set; } = 0;

        public int SortOrder { get; set; }

        public bool AutoApprove { get; set; }

        public int LeaveTypeId { get; init; }

        public int CompanyId { get; init; }

        public Company? Company { get; init; }

        public byte[]? RowVersion { get; init; }
        public ICollection<Leave> Leaves { get; set; } = [];

        public ICollection<Calendar> Calendar { get; set; } = [];
    }
}