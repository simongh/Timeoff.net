namespace Timeoff.Application.Settings
{
    public record SettingsViewModel : SettingsModel
    {
        public IEnumerable<bool> Schedule { get; init; } = null!;
        public LeaveTypeResult[] LeaveTypes { get; init; } = null!;

        public IEnumerable<ResultModels.CountryResult> Countries { get; set; }

        public IEnumerable<(string Id, string Name)> TimeZones { get; set; }

        public ResultModels.FlashResult? Result { get; set; }
    }
}