namespace Timeoff.Entities
{
    public class UserAllowanceAdjustment
    {
        public int Year { get; set; }

        public double Adjustment { get; set; }

        public double CarriedOverAllowance { get; set; }

        public DateTime CreatedAt { get; set; }

        public int UserAllowanceAdjustmentId { get; private set; }

        public int UserId { get; set; }

        public User User { get; set; } = null!;
        public byte[]? RowVersion { get; init; } = null!;
    }
}