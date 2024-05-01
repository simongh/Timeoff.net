namespace Timeoff.Entities
{
    public class UserAllowanceAdjustment
    {
        public int Year { get; init; }

        public double Adjustment { get; set; }

        public double CarriedOverAllowance { get; set; }

        public DateTime CreatedAt { get; init; }

        public int UserAllowanceAdjustmentId { get; init; }

        public int UserId { get; init; }

        public User User { get; init; } = null!;
        public byte[]? RowVersion { get; init; } = null!;
    }
}