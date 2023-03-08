namespace Timeoff.Services
{
    internal class TimeZoneService
    {
        private static readonly Lazy<IEnumerable<(string, string)>> _instance = new(Get);

        public static IEnumerable<(string, string)> TimeZones => _instance.Value;

        private static IEnumerable<(string, string)> Get()
        {
            return TimeZoneInfo.GetSystemTimeZones().Select(tz => (tz.Id, tz.DisplayName));
        }
    }
}