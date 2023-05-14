using System.Diagnostics.CodeAnalysis;

namespace WeatherAlerts.Clients;

[ExcludeFromCodeCoverage]
public class GisServerException : Exception
{
    public GisServerException(string message, Exception? innerException = default)
        : base(message, innerException)
    {
    }
}