namespace Timeoff.Entities
{
    public class Comment
    {
        public string EntityType { get; set; }

        public int EntityId { get; set; }

        public string Value { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public int CommentId { get; private set; }

        public int CompanyId { get; set; }

        public Company Company { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }
    }
}