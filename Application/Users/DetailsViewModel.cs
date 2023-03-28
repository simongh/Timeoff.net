namespace Timeoff.Application.Users
{
    public record DetailsViewModel : UserDetailsModelBase
    {
        public IEnumerable<ResultModels.ListItem> Departments { get; init; } = null!;

        public string DateFormat { get; init; } = null!;

        public string CompanyName { get; init; } = null!;

        public ResultModels.FlashResult? Messages { get; set; }
    }
}