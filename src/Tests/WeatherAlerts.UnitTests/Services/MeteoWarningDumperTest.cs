using Moq;
using UnitTestsCommon;
using WeatherAlerts.Commands;
using WeatherAlerts.Entities;
using WeatherAlerts.Models;
using WeatherAlerts.Services;

namespace WeatherAlerts.UnitTests.Services;

public class MeteoWarningDumperTest
{
    [Test]
    public async Task TestLoadAndDump_CallsDumpingCommand()
    {
        var meteoWarningService = new Mock<IMeteoWarningsService>();
        var mapper = MockHelper.MockAutoMapper<AutoMapperProfile>();
        var mediator = MockHelper.MockMediator();

        var result = new MeteoWarningsResult
        {
            Upcoming = new List<MeteoWarning>
            {
                new()
                {
                    Name = "Avalanches",
                    Type = MeteoWarningType.Avalanche
                }
            }.AsReadOnly(),
            In12Hours = new List<MeteoWarning>
            {
                new()
                {
                    Name = "Wind",
                    Type = MeteoWarningType.Wind
                }
            }.AsReadOnly(),
            NextDay = new List<MeteoWarning>
            {
                new()
                {
                    Name = "Thunder",
                    Type = MeteoWarningType.Thunder
                }
            }.AsReadOnly()
        };

        meteoWarningService.Setup(x => x.GetMeteoWarnings()).ReturnsAsync(result);

        var allItems = result.Upcoming.Union(result.In12Hours).Union(result.NextDay).ToList();

        var expectedItems = mapper.Map<List<MeteoWarningEntity>>(allItems);
        mediator.MockRequest(new DumpMeteoWarningsCommand(expectedItems));

        var dumper = new MeteoWarningsDumper(meteoWarningService.Object, mapper, mediator.Object);
        await dumper.LoadAndDump();
    }
}