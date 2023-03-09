namespace Timeoff.Types
{
    public class EmailOptions
    {
        public bool SendEmails { get; set; }

        public string Templates { get; set; } = ".\\Emails";

        public string Host { get; set; }

        public int Port { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }
    }
}