﻿namespace Timeoff.Types
{
    public class Options
    {
        public bool AllowNewAccountCreation { get; set; }

        public string SiteUrl { get; set; } = null!;

        public EmailOptions Email { get; set; } = new();

        public bool SelectLeaveType { get; set; }

        public string Secret { get; set; } = null!;
    }
}