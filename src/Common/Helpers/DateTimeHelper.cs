namespace Common.Helpers;

public static class DateTimeHelper
{
    public static System.DateTime FromUnixTimeToUtc(long unixTime)
    {
        var dtDateTime = new System.DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
        dtDateTime = dtDateTime.AddSeconds(unixTime).ToUniversalTime();
        return dtDateTime;
    }

    public static System.DateTime FromUnixTimeToLocal(long unixTime)
    {
        var dtDateTime = new System.DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Local);
        dtDateTime = dtDateTime.AddSeconds(unixTime).ToLocalTime();
        return dtDateTime;
    }
}