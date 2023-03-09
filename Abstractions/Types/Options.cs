namespace Timeoff.Types
{
    public class Options
    {
        public bool AllowNewAccountCreation { get; set; }

        public string SiteUrl { get; set; } = null!;

        public EmailOptions Email { get; set; } = new();
    }
}