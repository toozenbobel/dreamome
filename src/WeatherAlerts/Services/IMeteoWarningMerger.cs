using WeatherAlerts.Models;

namespace WeatherAlerts.Services;

internal interface IMeteoWarningMerger
{
    IEnumerable<MeteoWarning> Merge(IEnumerable<MeteoWarning> items);
}