namespace WeatherAlerts.Models.Http;

public record MeteoWarningHoursRange
{
    private MeteoWarningHoursRange(string value)
    {
        Value = value;
    }
    
    private const string UpcomingString = "0-12";
    private const string In12HoursString = "12-24";
    private const string NextDayString = "24-48";

    public static readonly MeteoWarningHoursRange Upcoming = new(UpcomingString);
    public static readonly MeteoWarningHoursRange In12Hours = new(In12HoursString);
    public static readonly MeteoWarningHoursRange NextDay = new(NextDayString);
  
    public string Value { get; }
}