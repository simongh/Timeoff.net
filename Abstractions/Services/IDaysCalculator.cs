namespace Timeoff.Services
{
    public interface IDaysCalculator
    {
        Task AdjustForHolidaysAsync(IEnumerable<(DateTime original, DateTime modified)> changed);

        IEnumerable<Entities.Calendar> CalculateDays(Entities.Leave leave, Entities.Schedule schedule, IEnumerable<DateTime> holidays);

        Task<IEnumerable<Entities.Calendar>> CalculateDaysAsync(Entities.Leave leave);
    }
}