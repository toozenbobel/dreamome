using System.Diagnostics.CodeAnalysis;

namespace Common.Helpers;

[ExcludeFromCodeCoverage]
public static class DateTimeProvider
{
    private static readonly TimeSpan RussiaStandardTimeOffset = TimeSpan.FromHours(+3);

    public static DateTime ToRussianStandardTime(DateTime dateTime)
    {
        var offset = new DateTimeOffset(dateTime).ToOffset(RussiaStandardTimeOffset);
        return offset.DateTime;
    }

    public static long UtcNowUnixTime => ((DateTimeOffset)UtcNow).ToUnixTimeMilliseconds();

    public static DateTime UtcNow =>
        DateTimeProviderContext.Current == null
            ? DateTime.UtcNow
            : DateTimeProviderContext.Current.ContextDateTimeNow();
}