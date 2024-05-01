namespace Timeoff.Entities
{
    public class Calendar
    {
        public int CalendarId { get; init; }

        public DateTime Date { get; set; }

        public int CompanyId { get; init; }
        public int? UserId { get; init; }
        public int? LeaveId { get; init; }

        public int? LeaveTypeId { get; init; }

        public LeavePart? LeavePart { get; init; }
        public bool IsHoliday { get; init; }

        public string? Name { get; set; }

        public virtual Company Company { get; init; } = null!;

        public virtual User? User { get; init; }

        public virtual Leave? Leave { get; init; }

        public virtual LeaveType? LeaveType { get; init; }
        public byte[]? RowVersion { get; init; } = null!;
    }
}