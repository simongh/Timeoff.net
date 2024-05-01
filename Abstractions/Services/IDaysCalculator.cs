﻿namespace Timeoff.Services
{
    public interface IDaysCalculator
    {
        Task AdjustForHolidaysAsync(IEnumerable<(DateTime original, DateTime modified)> changed);

        void CalculateDays(Entities.Leave leave, Entities.Schedule schedule, IEnumerable<DateTime> holidays);

        Task CalculateDaysAsync(Entities.Leave leave);
    }
}