using System.Collections.ObjectModel;
using WeatherAlerts.Models;
using WeatherAlerts.Models.Http;

namespace WeatherAlerts.Clients;

public interface IGisClient
{
    Task<ReadOnlyCollection<MeteoWarningItem>> GetMeteoWarnings(MeteoWarningHoursRange range);
}