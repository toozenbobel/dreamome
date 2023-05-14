using Common.Helpers;

namespace Common.UnitTests.Helpers;

public class DateTimeHelperTest
{
    [Test]
    public void TestFromUnitTimeToUtc_ReturnsProperTime()
    {
        const int unixTime = 1633083630;
        var expectedTime = new DateTime(2021, 10, 01, 10, 20, 30);

        var time = DateTimeHelper.FromUnixTimeToUtc(unixTime);

        Assert.That(time, Is.EqualTo(expectedTime));
    }
}