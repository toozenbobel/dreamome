namespace VoiceService.Services;

public interface ITtsService
{
    Task Speak(string text);
}