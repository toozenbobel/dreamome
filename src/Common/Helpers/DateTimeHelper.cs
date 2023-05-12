namespace Common.Helpers;

public static class DateTimeHelper
{
    public static DateTime FromUnixTimeToUtc(long unixTime)
    {
        var dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dtDateTime = dtDateTime.AddSeconds(unixTime).ToUniversalTime();
        return dtDateTime;
    }
    
    public static DateTime FromUnixTime(long unixTime)
    {
        var dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);
        dtDateTime = dtDateTime.AddSeconds(unixTime);
        return dtDateTime;
    }

    public static DateTime FromUnixTimeToLocal(long unixTime)
    {
        var dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Local);
        dtDateTime = dtDateTime.AddSeconds(unixTime).ToLocalTime();
        return dtDateTime;
    }
}