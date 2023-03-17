namespace Timeoff.ResultModels
{
    public record AllowanceByTimeViewModel
    {
        public IEnumerable<ListItem> Departments { get; init; }

        public IEnumerable<UserSummaryResult> Users { get; init; }

        public DateTime StartDate { get; init; }

        public DateTime EndDate { get; init; }

        public int? Department { get; init; }
    }
}