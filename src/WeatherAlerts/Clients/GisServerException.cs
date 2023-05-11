namespace WeatherAlerts.Clients;

public class GisServerException : Exception
{
    public GisServerException(string message, Exception? innerException = default)
        : base(message, innerException)
    {
    }
}