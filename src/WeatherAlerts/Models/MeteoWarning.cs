namespace WeatherAlerts.Models;

internal record MeteoWarning
{
    public DateTime Timestamp { get; init; } = DateTime.UtcNow;
    public DateTime StartsAt { get; init; }
    public DateTime EndsAt { get; init; } 
    public MeteoWarningLevel Level { get; init; }
    public MeteoWarningType Type { get; init; }
    public string? Name { get; init; }
    public string? Description { get; init; }
}