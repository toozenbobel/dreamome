using System.Collections.ObjectModel;

namespace WeatherAlerts.Models;

public record MeteoWarningsResult
{
    public required ReadOnlyCollection<MeteoWarning> Upcoming { get; init; }
    public required ReadOnlyCollection<MeteoWarning> In12Hours { get; init; }
    public required ReadOnlyCollection<MeteoWarning> NextDay { get; init; }
}