namespace Timeoff.Entities
{
    public class PublicHoliday
    {
        public required string Name { get; set; }

        public DateTime Date { get; set; }

        public int PublicHolidayId { get; init; }

        public int CompanyId { get; init; }

        public Company? Company { get; init; }

        public byte[]? RowVersion { get; init; }
    }
}