using Common.Helpers;
using Moq;
using WeatherAlerts.Clients;
using WeatherAlerts.Models;
using WeatherAlerts.Models.Http;
using WeatherAlerts.Services;

namespace WeatherAlerts.UnitTests.Services;

public class MeteoWarningsServiceTest
{
    [Test]
    public async Task TestGetMeteoWarnings_ReturnsAllWarnings()
    {
        var now = new DateTime(2020, 10, 01, 10, 20, 30, DateTimeKind.Local);
        using var _ = new DateTimeProviderContext(() => now);
        
        var client = new Mock<IGisClient>();

        var upcoming = new List<MeteoWarningItem>
        {
            new()
            {
                Name = "Avalanches",
                IconId = "309",
                From = 1601533230,
                To = 1601533230,
                Description = "Unit test"
            }
        }.AsReadOnly();

        var in12Hours = new List<MeteoWarningItem>
        {
            new()
            {
                Name = "Wind",
                IconId = "401",
                From = 1601533230,
                To = 1601533230,
                Description = "Unit test"
            }
        }.AsReadOnly();

        var nextDay = new List<MeteoWarningItem>
        {
            new()
            {
                Name = "Thunder",
                IconId = "303",
                From = 1601533230,
                To = 1601533230,
                Description = "Unit test"
            }
        }.AsReadOnly();

        client.Setup(x => x.GetMeteoWarnings(MeteoWarningHoursRange.Upcoming)).ReturnsAsync(upcoming);
        client.Setup(x => x.GetMeteoWarnings(MeteoWarningHoursRange.In12Hours)).ReturnsAsync(in12Hours);
        client.Setup(x => x.GetMeteoWarnings(MeteoWarningHoursRange.NextDay)).ReturnsAsync(nextDay);

        var expectedResult = new MeteoWarningsResult
        {
            Upcoming = new List<MeteoWarning>
            {
                new()
                {
                    Name = "Avalanches",
                    Type = MeteoWarningType.Avalanche,
                    Level = MeteoWarningLevel.Orange,
                    StartsAt = now.ToUniversalTime(),
                    EndsAt = now.ToUniversalTime(),
                    Description = "Unit test"
                }
            }.AsReadOnly(),
            In12Hours = new List<MeteoWarning>
            {
                new()
                {
                    Name = "Wind",
                    Type = MeteoWarningType.Wind,
                    Level = MeteoWarningLevel.Red,
                    StartsAt = now.ToUniversalTime(),
                    EndsAt = now.ToUniversalTime(),
                    Description = "Unit test"
                }
            }.AsReadOnly(),
            NextDay = new List<MeteoWarning>
            {
                new()
                {
                    Name = "Thunder",
                    Type = MeteoWarningType.Thunder,
                    Level = MeteoWarningLevel.Orange,
                    StartsAt = now.ToUniversalTime(),
                    EndsAt = now.ToUniversalTime(),
                    Description = "Unit test"
                }
            }.AsReadOnly()
        };

        var service = new MeteoWarningsService(client.Object);

        var result = await service.GetMeteoWarnings();
        
        Assert.Multiple(() =>
        {
            Assert.That(expectedResult.Upcoming, Is.EquivalentTo(result.Upcoming));
            Assert.That(expectedResult.In12Hours, Is.EquivalentTo(result.In12Hours));
            Assert.That(expectedResult.NextDay, Is.EquivalentTo(result.NextDay));
        });
    }
}