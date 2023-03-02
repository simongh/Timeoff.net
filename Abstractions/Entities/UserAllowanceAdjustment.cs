namespace Timeoff.Entities
{
    public class UserAllowanceAdjustment
    {
        public int Year { get; set; }

        public int Adjustment { get; set; }

        public int CarriedOverAllowance { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public int UserAllowAdjustmentId { get; private set; }

        public int UserId { get; set; }

        public User User { get; set; }
    }
}