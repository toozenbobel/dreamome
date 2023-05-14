using System.Net;
using System.Net.Http.Json;
using System.Net.Mime;
using System.Reflection;
using System.Text;
using Microsoft.Extensions.Options;
using RichardSzalay.MockHttp;
using WeatherAlerts.Clients;
using WeatherAlerts.Models.Http;
using WeatherAlerts.Settings;

namespace WeatherAlerts.UnitTests.Clients;

public class GisClientTest
{
    [Test]
    public async Task TestGetMeteoWarnings_OkResult()
    {
        const string resource = "WeatherAlerts.UnitTests.Resources.response.json";
        var assembly = Assembly.GetExecutingAssembly();
        
        await using var stream = assembly.GetManifestResourceStream(resource);
        using var reader = new StreamReader(stream!);
        var responseJson = await reader.ReadToEndAsync();

        var httpMock = new MockHttpMessageHandler();
        httpMock.Expect(GisApi.Data)
            .WithQueryString(new KeyValuePair<string, string>[]
            {
                new("id_fed", "5"),
                new("type", MeteoWarningHoursRange.Upcoming.Value),
                new("lang", "1")
            })
            .Respond(HttpStatusCode.OK, _ = new StringContent(responseJson, Encoding.Default, MediaTypeNames.Application.Json));

        var httpClient = httpMock.ToHttpClient();
        httpClient.BaseAddress = new Uri("http://unit.tests");

        var settings = new OptionsWrapper<AlertsSettings>(new AlertsSettings
        {
            FederalDistrictId = 5,
            RegionId = "59"
        });

        var expectedItems = new MeteoWarningItem[]
        {
            new()
            {
                From = 1684098000,
                To = 1684162800,
                IconId = "28",
                Name = "Пожарная опасность",
                Description = "Местами по северу области сохранится 4 класс пожароопасности лесов"
            }
        };

        var client = new GisClient(httpClient, settings);

        var result = await client.GetMeteoWarnings(MeteoWarningHoursRange.Upcoming);
        
        Assert.That(result, Is.EquivalentTo(expectedItems));
    }
}