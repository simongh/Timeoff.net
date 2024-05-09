namespace Timeoff.Entities
{
    public class UserAllowanceAdjustment
    {
        public int Year { get; init; }

        public double Adjustment { get; set; }

        public double CarriedOverAllowance { get; set; }

        public DateTime CreatedAt { get; init; } = DateTime.UtcNow;

        public int UserAllowanceAdjustmentId { get; init; }

        public int UserId { get; init; }

        public User? User { get; init; }
        public byte[]? RowVersion { get; init; }
    }
}