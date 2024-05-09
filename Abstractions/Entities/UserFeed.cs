namespace Timeoff.Entities
{
    public class UserFeed
    {
        public required string Name { get; set; }

        public required string FeedToken { get; set; }

        public FeedType Type { get; set; }

        public int UserFeedId { get; init; }

        public int UserId { get; init; }

        public User? User { get; init; }
        public byte[]? RowVersion { get; init; }
    }
}