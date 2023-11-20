namespace Timeoff.Types
{
    public record AllowanceCalculator
    {
        public double Allowance { get; init; }

        public double CarryOver { get; init; }

        public double DaysUsed { get; init; }

        public double Adjustment { get; init; }

        public bool IsAccrued { get; init; }

        public int YearStart { get; init; }

        public DateTime Start { get; init; }

        public DateTime? End { get; init; }

        public int Year { get; init; } = DateTime.Today.Year;

        private DateTime CalculationStart => Start.Year == Year ? Start : new DateTime(Year, 1, 1);

        private DateTime CalculationEnd => End?.Year <= Year ? End.Value : YearEnd(Year);

        private static DateTime YearEnd(int year) => new DateTime(year, 1, 1).AddYears(1).AddDays(-1);

        public double EmploymentRangeAdjustment
        {
            get
            {
                if (Start.Year != Year && (End == null || End.Value.Year != Year))
                    return 0;

                return Math.Round(-(Allowance - (Allowance * (CalculationEnd - CalculationStart).TotalDays / 365)));
            }
        }

        public double AccruedAdjustment
        {
            get
            {
                if (!IsAccrued)
                    return 0;

                var a = Allowance + CarryOver + EmploymentRangeAdjustment;
                var days = (CalculationEnd - CalculationStart).TotalDays;
                var delta = a * (CalculationEnd - DateTime.Today).TotalDays / days;

                return Math.Round(-((delta * 2) / 2));
            }
        }

        public double Total => Allowance + CarryOver + Adjustment + EmploymentRangeAdjustment + AccruedAdjustment;
    }
}