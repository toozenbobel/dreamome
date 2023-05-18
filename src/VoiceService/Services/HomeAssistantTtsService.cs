using HomeAssistantClient;
using Microsoft.Extensions.Options;

namespace VoiceService.Services;

internal class HomeAssistantTtsService : ITtsService
{
    private readonly IHomeAssistantClient _client;
    private readonly AliceSettings _settings;

    public HomeAssistantTtsService(IHomeAssistantClient client, IOptions<AliceSettings> settings)
    {
        _client = client;
        _settings = settings.Value;
    }
    
    public async Task Speak(string text)
    {
        var data = new HomeAssistantTtsModel
        {
            EntityId = _settings.EntityId,
            MediaContentId = text
        };
        
        await _client.CallService("media_player", "play_media", data);
    }
}