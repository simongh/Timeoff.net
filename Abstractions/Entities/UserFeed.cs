namespace Timeoff.Entities
{
    public class UserFeed
    {
        public string Name { get; set; } = null!;

        public string FeedToken { get; set; } = null!;

        public FeedType Type { get; set; }

        public int UserFeedId { get; init; }

        public int UserId { get; init; }

        public User User { get; init; } = null!;
        public byte[]? RowVersion { get; init; } = null!;
    }
}