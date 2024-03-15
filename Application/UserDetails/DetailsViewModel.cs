namespace Timeoff.Application.UserDetails
{
    public record DetailsViewModel : Types.UserDetailsModelBase
    {
        public int Team { get => TeamId; init => TeamId = value; }

        public IEnumerable<ResultModels.ListItem> Teams { get; init; } = null!;

        public string DateFormat { get; init; } = null!;

        public string CompanyName { get; init; } = null!;

        public ResultModels.FlashResult? Messages { get; set; }
    }
}