using Newtonsoft.Json;

namespace WeatherAlerts.Models.Http;

internal record MeteoWarningItem
{
    [JsonProperty("0")]
    public int From { get; set; }

    [JsonProperty("1")]
    public int To { get; set; }

    [JsonProperty("2")]
    public string? IconId { get; set; }

    [JsonProperty("3")]
    public string? Name { get; set; }

    [JsonProperty("4")]
    public string? Description { get; set; }
}