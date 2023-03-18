namespace Timeoff.Types
{
    public abstract record SettingsModel
    {
        public string Name { get; init; }

        public string Country { get; init; }

        public string DateFormat { get; init; }

        public string TimeZone { get; init; }

        public int CarryOver { get; init; }

        public bool ShowHoliday { get; init; }

        public bool HideTeamView { get; init; }
    }
}