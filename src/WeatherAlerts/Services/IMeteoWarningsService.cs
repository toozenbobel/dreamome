using WeatherAlerts.Models;

namespace WeatherAlerts.Services;

public interface IMeteoWarningsService
{
    Task<MeteoWarningsResult> GetMeteoWarnings();
}