namespace Timeoff.Application.Settings
{
    public abstract record SettingsModel
    {
        public string Name { get; init; } = null!;

        public string Country { get; init; } = null!;

        public string DateFormat { get; init; } = null!;

        public string TimeZone { get; init; } = null!;

        public int CarryOver { get; init; }

        public bool ShowHoliday { get; init; }

        public bool HideTeamView { get; init; }
    }
}