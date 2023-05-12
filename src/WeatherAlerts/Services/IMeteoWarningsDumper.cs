namespace WeatherAlerts.Services;

public interface IMeteoWarningsDumper
{
    Task LoadAndDump();
}