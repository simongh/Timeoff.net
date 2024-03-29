﻿namespace Timeoff.Application.Calendar
{
    public record CalendarViewModel
    {
        public int CurrentYear { get; init; }

        public bool ShowFullYear { get; init; }

        public string Name { get; init; } = null!;

        public int NextYear => CurrentYear + 1;

        public int LastYear => CurrentYear - 1;

        public ResultModels.CalendarResult Calendar { get; init; } = null!;

        public ResultModels.AllowanceSummaryResult AllowanceSummary { get; init; } = null!;

        public StatsResult Statistics { get; init; } = null!;

        public IEnumerable<ResultModels.LeaveRequestedResult> LeaveRequested { get; init; } = null!;
    }
}