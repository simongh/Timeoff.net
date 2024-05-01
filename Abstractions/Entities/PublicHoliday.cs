namespace Timeoff.Entities
{
    public class PublicHoliday
    {
        public string Name { get; set; } = null!;

        public DateTime Date { get; set; }

        public int PublicHolidayId { get; init; }

        public int CompanyId { get; init; }

        public Company Company { get; init; } = null!;

        public byte[]? RowVersion { get; init; } = null!;
    }
}