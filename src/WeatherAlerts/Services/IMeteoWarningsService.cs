using WeatherAlerts.Models;

namespace WeatherAlerts.Services;

internal interface IMeteoWarningsService
{
    Task<MeteoWarningsResult> GetMeteoWarnings();
}