namespace VoiceService.Model;

public record TextToSpeakRequest
{
    public string? Text { get; init; }
}