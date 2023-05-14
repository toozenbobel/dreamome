using System.Collections.ObjectModel;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Serilog;
using WeatherAlerts.Models;
using WeatherAlerts.Models.Http;
using WeatherAlerts.Settings;

namespace WeatherAlerts.Clients;

internal class GisClient : IGisClient
{
    private readonly HttpClient _httpClient;
    private readonly AlertsSettings _settings;

    public GisClient(HttpClient httpClient, IOptions<AlertsSettings> settings)
    {
        _httpClient = httpClient;
        _settings = settings.Value;
    }
    
    public async Task<ReadOnlyCollection<MeteoWarningItem>> GetMeteoWarnings(MeteoWarningHoursRange range)
    {
        var url = $"{GisApi.Data}?id_fed={_settings.FederalDistrictId}&type={range.Value}&lang=1";

        var responseMessage = await _httpClient.GetAsync(url);

        await CheckErrorCode(responseMessage);  

        var response = await responseMessage.Content.ReadAsStringAsync();

        var jObject = JsonConvert.DeserializeObject<JObject>(response);

        if (jObject != null && jObject.TryGetValue(_settings.RegionId, out var jToken))
        {
            var items = jToken.Values().Select(x => x.ToObject<MeteoWarningItem>()!).ToList();
            return items.AsReadOnly();
        }
        
        Log.Warning("Couldn't get warnings. Region not found");
        return new List<MeteoWarningItem>().AsReadOnly();
    }

    private static async Task CheckErrorCode(HttpResponseMessage response)
    {
        if (response == null)
            throw new GisServerException("Response is null or empty");

        if (!response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            throw new GisServerException(content);
        }
    }
}