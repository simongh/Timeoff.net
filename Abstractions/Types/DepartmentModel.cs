﻿namespace Timeoff.Types
{
    public abstract record DepartmentModel
    {
        public string Name { get; init; }
        public double Allowance { get; init; }
        public bool IncludePublicHolidays { get; init; }

        public bool IsAccruedAllowance { get; init; }

        public int ManagerId { get; init; }
    }
}