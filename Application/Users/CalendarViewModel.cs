namespace Timeoff.Application.Users
{
    public record CalendarViewModel : Types.UserModel
    {
        public int CurrentYear { get; init; }

        public int NextYear => CurrentYear + 1;

        public int PreviousYear => CurrentYear - 1;

        public double Allowance { get; init; }

        public double CarryOver { get; init; }

        public double Adjustment { get; init; }

        public double EmploymentAdjustment { get; init; }

        public bool IsAccrued { get; init; }

        public double AccruedAdjustment { get; init; }

        public IEnumerable<ResultModels.LeaveSummaryResult> Leaves { get; init; } = null!;

        public double Used => Leaves
            .Where(i => i.AffectsAllowance)
            .Sum(i => i.Total);

        public double Total => Allowance + CarryOver + Adjustment + EmploymentAdjustment + AccruedAdjustment;

        public double Available => Total - Used;

        public IEnumerable<ResultModels.CalendarMonthResult> Calendar { get; init; } = null!;
    }
}