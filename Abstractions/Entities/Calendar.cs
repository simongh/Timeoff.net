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

        public Company? Company { get; init; }

        public User? User { get; init; }

        public Leave? Leave { get; init; }

        public LeaveType? LeaveType { get; init; }
        public byte[]? RowVersion { get; init; }
    }
}