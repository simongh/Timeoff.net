namespace Timeoff.Types
{
    public class AllowanceCalculator
    {
        private Entities.UserAllowanceAdjustment _adjustment;

        public double Allowance { get; init; }

        public Entities.UserAllowanceAdjustment Adjustment
        {
            get => _adjustment;
            init => _adjustment = value ?? new Entities.UserAllowanceAdjustment();
        }

        public double DaysUsed { get; init; }

        public bool Acrrue { get; init; }

        public int YearStart { get; init; }

        public DateTime Start { get; init; }

        public DateTime? End { get; init; }

        private int CurrentYear => DateTime.Today.Year;

        private DateTime CalculationStart => Start.Year == CurrentYear ? Start : new DateTime(CurrentYear, 1, 1);

        private DateTime CalculationEnd => End?.Year <= CurrentYear ? End.Value : YearEnd(CurrentYear);

        private static DateTime YearEnd(int year) => new DateTime(year, 1, 1).AddYears(1).AddDays(-1);

        private double EmploymentRangeAdjustment
        {
            get
            {
                if (Start.Year != CurrentYear && (End == null || End.Value.Year > CurrentYear))
                    return 0;

                return -(Allowance - (Allowance * (CalculationEnd - CalculationStart).TotalDays / 365));
            }
        }

        private double AccruedAdjustment
        {
            get
            {
                var a = Allowance + Adjustment.CarriedOverAllowance + EmploymentRangeAdjustment;
                var days = (CalculationEnd - CalculationStart).TotalDays;
                var delta = a * (CalculationEnd - DateTime.Today).TotalDays / days;

                return -((delta * 2) / 2);
            }
        }
    }
}