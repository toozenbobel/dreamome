namespace WeatherAlerts.Settings;

internal record AlertsSettings
{
    public int FederalDistrictId { get; init; }
    public string RegionId { get; init; } = "59";
    
    public TimeSpan UpdateInterval { get; init; }
}