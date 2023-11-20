namespace Timeoff.Entities
{
    public class UserFeed
    {
        public string Name { get; set; }

        public string FeedToken { get; set; }

        public FeedType Type { get; set; }

        public int UserFeedId { get; private set; }

        public int UserId { get; set; }

        public User User { get; set; } = null!;
        public byte[]? RowVersion { get; init; } = null!;
    }
}