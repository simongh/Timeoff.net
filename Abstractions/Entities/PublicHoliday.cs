namespace Timeoff.Entities
{
    public class PublicHoliday
    {
        public string Name { get; set; } = null!;

        public DateTime Date { get; set; }

        public int PublicHolidayId { get; private set; }

        public int CompanyId { get; set; }

        public Company Company { get; set; } = null!;
    }
}