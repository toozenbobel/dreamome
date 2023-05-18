namespace VoiceService.Services;

public record HomeAssistantTtsModel
{
    public required string? EntityId { get; init; }

    public string MediaContentType => "text";
    
    public required string? MediaContentId { get; init; }
}