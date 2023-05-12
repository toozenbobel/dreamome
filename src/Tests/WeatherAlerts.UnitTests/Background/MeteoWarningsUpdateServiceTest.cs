using Microsoft.Extensions.Options;
using Moq;
using UnitTestsCommon;
using WeatherAlerts.Background;
using WeatherAlerts.Services;
using WeatherAlerts.Settings;

namespace WeatherAlerts.UnitTests.Background;

public class MeteoWarningsUpdateServiceTest
{
    [Test]
    public async Task Execute_DumpsWarningToInflux()
    {
        var dumper = new Mock<IMeteoWarningsDumper>();

        var serviceProvider = MockHelper.MockServiceProvider();
        serviceProvider.Setup(x => x.GetService(typeof(IMeteoWarningsDumper))).Returns(dumper.Object);

        var settings = new OptionsWrapper<AlertsSettings>(new AlertsSettings
        {
            UpdateInterval = TimeSpan.FromMinutes(1)
        });

        var cts = new CancellationTokenSource();

        var service = new MeteoWarningsUpdateService(serviceProvider.Object, settings);

        await service.StartAsync(cts.Token);
        await service.StopAsync(cts.Token);
        
        dumper.Verify(x => x.LoadAndDump(), Times.Once);
    }
}