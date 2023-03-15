namespace Timeoff.Types
{
    public abstract record BankHolidayModel
    {
        public int? Id { get; init; }

        public string Name { get; init; }

        public DateTime Date { get; init; }
    }
}