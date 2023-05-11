namespace WeatherAlerts.Models.Http;

internal record MeteoWarningHoursRange(string Value)
{
    private const string UpcomingString = "0-12";
    private const string In12HoursString = "12-24";
    private const string NextDayString = "24-48";

    public static MeteoWarningHoursRange Upcoming = new(UpcomingString);
    public static MeteoWarningHoursRange In12Hours = new(In12HoursString);
    public static MeteoWarningHoursRange NextDay = new(NextDayString);
}