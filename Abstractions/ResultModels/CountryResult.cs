using System.Text.Json.Serialization;

namespace Timeoff.ResultModels
{
    public readonly record struct CountryResult(
        [property: JsonPropertyName("code")] string Code,
        [property: JsonPropertyName("name")] string Name);
}