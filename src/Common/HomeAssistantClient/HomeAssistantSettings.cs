using Newtonsoft.Json;

namespace HomeAssistantClient;

public record HomeAssistantSettings
{
    public string? Token { get; init; }
    
    public string? Host { get; init; }
}