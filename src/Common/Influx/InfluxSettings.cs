using System.Diagnostics.CodeAnalysis;

namespace Common.Influx;

[ExcludeFromCodeCoverage]
public record InfluxSettings
{
    public string? Token { get; init; }
    public string? Host { get; init; }
    
    public string? OrgId { get; init; }
}