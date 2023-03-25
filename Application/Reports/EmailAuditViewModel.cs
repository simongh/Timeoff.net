namespace Timeoff.Application.Reports
{
    public record EmailAuditViewModel
    {
        public DateTime? Start { get; init; }
        public DateTime? End { get; init; }

        public int? UserId { get; init; }

        public string DateFormat { get; init; } = null!;

        public IEnumerable<ResultModels.ListItem> Users { get; init; } = null!;

        public IEnumerable<ResultModels.EmailResult> Emails { get; init; } = null!;

        public ResultModels.PagerResult Pager { get; init; } = null!;

        public bool ShowReset => Start.HasValue || End.HasValue || UserId.HasValue;
    }
}