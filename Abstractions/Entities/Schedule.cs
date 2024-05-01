namespace Timeoff.Entities
{
    public class Schedule
    {
        public WorkingDay Monday { get; set; }

        public WorkingDay Tuesday { get; set; }

        public WorkingDay Wednesday { get; set; }

        public WorkingDay Thursday { get; set; }

        public WorkingDay Friday { get; set; }

        public WorkingDay Saturday { get; set; } = WorkingDay.None;
        public WorkingDay Sunday { get; set; } = WorkingDay.None;

        public int ScheduleId { get; init; }

        public int? UserId { get; init; }

        public User? User { get; init; }

        public int? CompanyId { get; init; }

        public Company? Company { get; init; }

        public byte[]? RowVersion { get; init; } = null!;
    }
}