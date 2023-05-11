using System.Collections.ObjectModel;
using WeatherAlerts.Models;
using WeatherAlerts.Models.Http;

namespace WeatherAlerts.Clients;

internal interface IGisClient
{
    Task<ReadOnlyCollection<MeteoWarningItem>> GetMeteoWarnings(MeteoWarningHoursRange range);
}