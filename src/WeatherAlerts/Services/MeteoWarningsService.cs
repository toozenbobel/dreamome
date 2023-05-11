using System.Collections.ObjectModel;
using WeatherAlerts.Clients;
using WeatherAlerts.Models;
using WeatherAlerts.Models.Http;

namespace WeatherAlerts.Services;

internal class MeteoWarningsService : IMeteoWarningsService
{
    private readonly IGisClient _client;

    public MeteoWarningsService(IGisClient client)
    {
        _client = client;
    }

    public async Task<MeteoWarningsResult> GetMeteoWarnings()
    {
        var upcomingWarnings = await _client.GetMeteoWarnings(MeteoWarningHoursRange.Upcoming);
        var in12HoursWarnings = await _client.GetMeteoWarnings(MeteoWarningHoursRange.In12Hours);
        var nextDayWarnings = await _client.GetMeteoWarnings(MeteoWarningHoursRange.NextDay);

        return new MeteoWarningsResult
        {
            Upcoming = upcomingWarnings.Select(MeteoWarningMapper.Map).ToList().AsReadOnly(),
            In12Hours = in12HoursWarnings.Select(MeteoWarningMapper.Map).ToList().AsReadOnly(),
            NextDay = nextDayWarnings.Select(MeteoWarningMapper.Map).ToList().AsReadOnly(),
        };
    }
}