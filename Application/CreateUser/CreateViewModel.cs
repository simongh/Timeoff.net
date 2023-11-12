namespace Timeoff.Application.CreateUser
{
    public record CreateViewModel : Types.UserDetailsModelBase
    {
        public string CompanyName { get; init; } = null!;

        public IEnumerable<ResultModels.ListItem> Teams { get; init; } = null!;

        public string DateFormat { get; init; } = null!;

        public ResultModels.FlashResult? Messages { get; set; }
    }
}