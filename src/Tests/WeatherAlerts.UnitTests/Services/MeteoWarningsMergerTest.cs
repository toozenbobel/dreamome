using Common.Helpers;
using WeatherAlerts.Models;
using WeatherAlerts.Services;

namespace WeatherAlerts.UnitTests.Services;

public class MeteoWarningsMergerTest
{
    [Test]
    public void TestMerge_ReturnsMergedEntities()
    {
        var now = DateTime.UtcNow;

        using var _ = new DateTimeProviderContext(() => now);

        var singleWarning = new MeteoWarning
        {
            Description = "Should not be merged",
            Name = "Single",
            StartsAt = new DateTime(2000, 02, 01),
            EndsAt = new DateTime(2000, 02, 02),
            Level = MeteoWarningLevel.Yellow,
            Type = MeteoWarningType.Icy
        };

        var warningsToMerge = new[]
        {
            new MeteoWarning
            {
                Description = "Min level desc",
                Name = "ToMerge",
                StartsAt = new DateTime(2001, 02, 02),
                EndsAt = new DateTime(2001, 02, 03),
                Level = MeteoWarningLevel.Yellow,
                Type = MeteoWarningType.Thunder
            },
            new MeteoWarning
            {
                Description = "Max level desc",
                Name = "ToMerge",
                StartsAt = new DateTime(2001, 02, 01),
                EndsAt = new DateTime(2001, 02, 02),
                Level = MeteoWarningLevel.Red,
                Type = MeteoWarningType.Thunder
            },
            new MeteoWarning
            {
                Description = "Longest ends at",
                Name = "ToMerge",
                StartsAt = new DateTime(2001, 02, 02),
                EndsAt = new DateTime(2001, 02, 05),
                Level = MeteoWarningLevel.Yellow,
                Type = MeteoWarningType.Thunder
            }
        };

        var expectedResult = new[]
        {
            singleWarning,
            new MeteoWarning
            {
                Description = "Max level desc",
                Name = "ToMerge",
                StartsAt = new DateTime(2001, 02, 01),
                EndsAt = new DateTime(2001, 02, 05),
                Level = MeteoWarningLevel.Red,
                Type = MeteoWarningType.Thunder
            }
        };

        var warnings = new List<MeteoWarning>
        {
            singleWarning
        };
        warnings.AddRange(warningsToMerge);

        var merger = new MeteoWarningMerger();
        var result = merger.Merge(warnings);
        
        Assert.That(result, Is.EquivalentTo(expectedResult));
    }
}