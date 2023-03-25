namespace Timeoff.Types
{
    public abstract record PublicHolidayModel
    {
        public int? Id { get; init; }

        public string Name { get; init; } = null!;

        public DateTime Date { get; init; }
    }
}