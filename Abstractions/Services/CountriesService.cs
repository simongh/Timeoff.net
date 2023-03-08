using System.Text.Json;

namespace Timeoff.Services
{
    internal class CountriesService
    {
        private static readonly Lazy<IEnumerable<ResultModels.CountryResult>> _instance = new(Get);

        public static IEnumerable<ResultModels.CountryResult> Countries => _instance.Value;

        private static IEnumerable<ResultModels.CountryResult> Get()
        {
            var strm = typeof(CountriesService).Assembly.GetManifestResourceStream("Timeoff.countries.json");
            return JsonSerializer.Deserialize<IEnumerable<ResultModels.CountryResult>>(strm!)!;
        }
    }
}