namespace Timeoff.Application.Users
{
    public record UsersViewModel
    {
        public string CompanyName { get; init; } = null!;

        public int? TeamId { get; init; }

        public IEnumerable<ResultModels.ListItem> Teams { get; init; } = null!;

        public IEnumerable<UserInfoResult> Users { get; init; } = null!;

        public ResultModels.FlashResult? Messages { get; set; }
    }
}