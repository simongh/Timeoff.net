﻿namespace Timeoff.Types
{
    public record TeamModel
    {
        public string Name { get; init; } = null!;
        public double Allowance { get; init; }
        public bool IncludePublicHolidays { get; init; }

        public bool IsAccruedAllowance { get; init; }

        public int? ManagerId { get; init; }

        public ResultModels.ListResult? Manager { get; init; }
    }
}