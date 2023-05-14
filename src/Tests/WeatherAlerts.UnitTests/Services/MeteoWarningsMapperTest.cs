using Common.Helpers;
using WeatherAlerts.Models;
using WeatherAlerts.Models.Http;
using WeatherAlerts.Services;

namespace WeatherAlerts.UnitTests.Services;

public class MeteoWarningsMapperTest
{
    private static readonly DateTime Now = new(2020, 01, 02);
    
    [TestCaseSource(nameof(MapperTestData))]
    public void TestMap_ReturnsValidEntity(MeteoWarningItem item, MeteoWarning expectedResult)
    {
        using var _ = new DateTimeProviderContext(() => Now);
        
        var result = MeteoWarningMapper.Map(item);
        
        Assert.That(result, Is.EqualTo(expectedResult));
    }
    
    private static IEnumerable<TestCaseData> MapperTestData => GetTestCases();

    private static IEnumerable<TestCaseData> GetTestCases()
    {
        const string description = "This is a test event";
        const string name = "TestEvent";
        const int startsAt = 949386030;
        const int endsAt = 949476153;

        var expectedStartsAt = DateTimeHelper.FromUnixTime(startsAt);
        var expectedEndsAt = DateTimeHelper.FromUnixTime(endsAt);

        var item = new MeteoWarningItem
        {
            Description = description,
            From = startsAt,
            To = endsAt,
            Name = name
        };

        var expectedItem = new MeteoWarning
        {
            Timestamp = Now,
            StartsAt = expectedStartsAt,
            EndsAt = expectedEndsAt,
            Description = description,
            Name = name,
            Type = MeteoWarningType.General,
            Level = MeteoWarningLevel.Trivial
        };

        yield return new TestCaseData(item, expectedItem);

        item = item with
        {
            IconId = "000"
        };

        expectedItem = expectedItem with
        {
            Level = MeteoWarningLevel.Trivial,
            Type = MeteoWarningType.None
        };

        yield return new TestCaseData(item, expectedItem);
        
        item = item with
        {
            IconId = "001"
        };

        expectedItem = expectedItem with
        {
            Level = MeteoWarningLevel.Trivial,
            Type = MeteoWarningType.None
        };
        
        yield return new TestCaseData(item, expectedItem);
        
        item = item with
        {
            IconId = "003"
        };

        expectedItem = expectedItem with
        {
            Level = MeteoWarningLevel.Trivial,
            Type = MeteoWarningType.Thunder
        };
        
        yield return new TestCaseData(item, expectedItem);
        
        item = item with
        {
            IconId = "103"
        };

        expectedItem = expectedItem with
        {
            Level = MeteoWarningLevel.Trivial,
            Type = MeteoWarningType.Thunder
        };
        
        yield return new TestCaseData(item, expectedItem);

        item = item with
        {
            IconId = "201"
        };

        expectedItem = expectedItem with
        {
            Level = MeteoWarningLevel.Yellow,
            Type = MeteoWarningType.Wind
        };
        
        yield return new TestCaseData(item, expectedItem);
        
        item = item with
        {
            IconId = "301"
        };

        expectedItem = expectedItem with
        {
            Level = MeteoWarningLevel.Orange,
            Type = MeteoWarningType.Wind
        };
        
        yield return new TestCaseData(item, expectedItem);
        
        item = item with
        {
            IconId = "401"
        };

        expectedItem = expectedItem with
        {
            Level = MeteoWarningLevel.Red,
            Type = MeteoWarningType.Wind
        };
        
        yield return new TestCaseData(item, expectedItem);

        var allWeatherTypes = Enum.GetValues<MeteoWarningType>();

        foreach (var weatherType in allWeatherTypes)
        {
            item = item with
            {
                IconId = $"2{(int)weatherType:00}"
            };

            expectedItem = expectedItem with
            {
                Level = MeteoWarningLevel.Yellow,
                Type = weatherType
            };
            
            yield return new TestCaseData(item, expectedItem);
        }
    }
}