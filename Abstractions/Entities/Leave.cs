namespace Timeoff.Entities
{
    public class Leave
    {
        public LeaveStatus Status { get; set; }

        public string? EmployeeComment { get; set; }

        public string? ApproverComment { get; set; }

        public DateTime? DecidedOn { get; set; }

        public DateTime DateStart { get; set; }

        public LeavePart DayPartStart { get; set; } = LeavePart.All;

        public DateTime DateEnd { get; set; }

        public LeavePart DayPartEnd { get; set; } = LeavePart.All;

        public int LeaveId { get; private set; }

        public int UserId { get; set; }

        public User User { get; set; }

        public int ApproverId { get; set; }

        public User Approver { get; set; }

        public int LeaveTypeId { get; set; }

        public LeaveType LeaveType { get; set; }

        //public ICollection<Comment> Comments { get; set; }
    }
}