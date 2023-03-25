namespace Timeoff.Application.Settings
{
    public record SettingsViewModel : Types.SettingsModel
    {
        public IEnumerable<bool> Schedule { get; init; } = null!;
        public ResultModels.LeaveTypeResult[] LeaveTypes { get; init; } = null!;

        public int CurrentYear => DateTime.Today.Year;

        public int LastYear => CurrentYear - 1;

        public IEnumerable<int> CarryOverDays
        {
            get
            {
                for (var i = 0; i < 22; i++)
                {
                    yield return i;
                }
                yield return 1000;
            }
        }

        public IEnumerable<string> DateFormatOptions
        {
            get
            {
                yield return "yyyy-MM-dd";
                yield return "yyyy/MM/dd";
                yield return "dd MMM yy";
                yield return "dd/MM/yy";
                yield return "dd/MM/yyyy";
                yield return "MM/dd/yy";
            }
        }

        public IEnumerable<ResultModels.CountryResult> Countries { get; set; }

        public IEnumerable<(string Id, string Name)> TimeZones { get; set; }

        public ResultModels.FlashResult? Result { get; set; }
    }
}