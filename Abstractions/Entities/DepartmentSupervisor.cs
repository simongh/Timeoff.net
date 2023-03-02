namespace Timeoff.Entities
{
    public class DepartmentSupervisor
    {
        public DateTimeOffset CreatedAt { get; set; }

        public int DepartmentId { get; set; }

        public Department Department { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }
    }
}