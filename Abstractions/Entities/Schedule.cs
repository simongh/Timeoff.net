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

        public int ScheduleId { get; private set; }

        public int? UserId { get; set; }

        public User? User { get; set; }

        public int? CompanyId { get; set; }

        public Company? Company { get; set; }
    }
}