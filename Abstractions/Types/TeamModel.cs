namespace Timeoff.Types
{
    public abstract record TeamModel
    {
        public string Name { get; init; } = null!;
        public double Allowance { get; init; }
        public bool IncludePublicHolidays { get; init; }

        public bool IsAccruedAllowance { get; init; }

        public int ManagerId { get; init; }

        public ResultModels.UserResult Manager { get; init; } = null!;
    }
}