using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ExoScraper.Domain.Selectors;

[JsonConverter(typeof(StringEnumConverter))]
public enum PageType
{
    Dynamic,
    Static
}