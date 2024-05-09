namespace Timeoff.Entities
{
    public class Leave
    {
        public LeaveStatus Status { get; set; }

        public string? EmployeeComment { get; init; }

        public string? ApproverComment { get; init; }

        public DateTime? DecidedOn { get; set; }

        public DateTime DateStart { get; set; }

        public LeavePart DayPartStart { get; set; } = LeavePart.All;

        public DateTime DateEnd { get; set; }

        public LeavePart DayPartEnd { get; set; } = LeavePart.All;

        public double Days { get; set; }

        public int LeaveId { get; init; }

        public int UserId { get; init; }

        public User? User { get; init; }

        public int ApproverId { get; init; }

        public User? Approver { get; init; }

        public int LeaveTypeId { get; init; }

        public LeaveType? LeaveType { get; init; }

        public DateTime CreatedAt { get; init; } = DateTime.Now;

        public ICollection<Calendar> Calendar { get; init; } = [];

        public byte[]? RowVersion { get; init; }
    }
}