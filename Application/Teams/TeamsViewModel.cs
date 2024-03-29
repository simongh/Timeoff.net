﻿namespace Timeoff.Application.Teams
{
    public record TeamsViewModel
    {
        public IEnumerable<TeamResult> Teams { get; init; } = null!;

        public IEnumerable<ResultModels.ListItem> Users { get; init; } = null!;

        public IEnumerable<double> Allowances
        {
            get
            {
                for (double i = 0; i <= 50; i += 0.5)
                {
                    yield return i;
                }
            }
        }

        public ResultModels.FlashResult? Result { get; set; }

        public string GetName(int userId)
        {
            return Users.First(u => u.Id == userId).Value;
        }
    }
}