using Microsoft.AspNetCore.Mvc;
using VoiceService.Model;
using VoiceService.Services;

namespace VoiceService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TtsController : ControllerBase
{
    private readonly ITtsService _ttsService;

    public TtsController(ITtsService ttsService)
    {
        _ttsService = ttsService;
    }

    [HttpPost]
    public async Task<ActionResult> Speak([FromBody] TextToSpeakRequest request)
    {
        if (request.Text == null)
        {
            return BadRequest("Text is null or empty");
        }

        await _ttsService.Speak(request.Text);

        return Ok();
    }
}