using System.Diagnostics.CodeAnalysis;
using InfluxDB.Client.Core;
using WeatherAlerts.Models;

namespace WeatherAlerts.Entities;

[ExcludeFromCodeCoverage]
[Measurement("weatheralert")]
internal record MeteoWarningEntity
{
    [Column(IsTimestamp = true)]   
    public DateTime Timestamp { get; init; }
    
    [Column("startsAt")]
    public DateTime StartsAt { get; init; }
    
    [Column("startsAt")]
    public DateTime EndsAt { get; init; }
    
    [Column("level", IsTag = true)]
    public MeteoWarningLevel Level { get; init; }
    
    [Column("type", IsTag = true)]
    public MeteoWarningType Type { get; init; }
    
    [Column("name")]
    public string? Name { get; init; }
    
    [Column("description")]
    public string? Description { get; init; } 
}