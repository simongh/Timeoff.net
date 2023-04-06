namespace Timeoff.Application.Users
{
    public record CreateViewModel : UserDetailsModelBase
    {
        public string CompanyName { get; init; } = null!;

        public IEnumerable<ResultModels.ListItem> Teams { get; init; } = null!;

        public string DateFormat { get; init; } = null!;

        public ResultModels.FlashResult? Messages { get; set; }
    }
}