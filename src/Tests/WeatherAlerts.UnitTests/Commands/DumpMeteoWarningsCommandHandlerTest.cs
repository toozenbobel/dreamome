using Common.Influx;
using Moq;
using WeatherAlerts.Commands;
using WeatherAlerts.Entities;
using WeatherAlerts.Models;
using WeatherAlerts.Settings;

namespace WeatherAlerts.UnitTests.Commands;

public class DumpMeteoWarningsCommandHandlerTest
{
    [Test]
    public async Task TestHandle_WritesMeasurements()
    {
        var warnings = new[]
        {
            new MeteoWarningEntity
            {
                Type = MeteoWarningType.Avalanche,
                Description = "Avalanche danger",
                Level = MeteoWarningLevel.Yellow
            },
            new MeteoWarningEntity
            {
                Type = MeteoWarningType.Flood,
                Description = "Flood danger",
                Level = MeteoWarningLevel.Orange
            }
        };

        var client = new Mock<IInfluxClient>();

        var command = new DumpMeteoWarningsCommand(warnings);
        var handler = new DumpMeteoWarningsCommand.DumpMeteoWarningsCommandHandler(client.Object);

        await handler.Handle(command, CancellationToken.None);
        
        client.Verify(x => x.WriteMeasurements(warnings, InfluxBuckets.MainBucketName), Times.Once);
    }
}