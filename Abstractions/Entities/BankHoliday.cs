namespace Timeoff.Entities
{
    public class BankHoliday
    {
        public string Name { get; set; }

        public DateTime Date { get; set; }

        public int BankHolidayId { get; private set; }

        public int CompanyId { get; set; }

        public Company Company { get; set; }
    }
}