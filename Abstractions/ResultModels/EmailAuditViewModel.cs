namespace Timeoff.ResultModels
{
    public record EmailAuditViewModel
    {
        public DateTime? Start { get; init; }
        public DateTime? End { get; init; }

        public int? UserId { get; init; }

        public string DateFormat { get; init; } = null!;

        public IEnumerable<ListItem> Users { get; init; } = null!;

        public IEnumerable<EmailResult> Emails { get; init; } = null!;

        public PagerResult Pager { get; init; } = null!;

        public bool ShowReset => Start.HasValue || End.HasValue || UserId.HasValue;
    }
}