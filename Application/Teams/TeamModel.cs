﻿namespace Timeoff.Application.Teams
{
    public abstract record TeamModel
    {
        public string Name { get; init; } = null!;
        public double Allowance { get; init; }
        public bool IncludePublicHolidays { get; init; }

        public bool IsAccruedAllowance { get; init; }
    }
}