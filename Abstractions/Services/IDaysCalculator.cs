namespace Timeoff.Services
{
    public interface IDaysCalculator
    {
        Task AdjustForHolidaysAsync(IEnumerable<(DateTime original, DateTime modified)> changed, int companyId);

        void CalculateDays(Entities.Leave leave);
    }
}