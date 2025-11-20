using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Abstractions.Services;

namespace Identity.Api.Controllers.Modules.Assistant;

public record ChatRequestDto(string Message);
public record ChatResponseDto(string Reply);

//[Authorize]
[Route("api/[controller]")]
[ApiController]
public class ChatbotController : ControllerBase
{
    private readonly IChatbotService _chatbotService;

    public ChatbotController(IChatbotService chatbotService)
    {
        _chatbotService = chatbotService;
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] ChatRequestDto request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Message))
            return BadRequest("Message is required.");

        try
        {
            var reply = await _chatbotService.GetReplyAsync(request.Message, cancellationToken);
            return Ok(new ChatResponseDto(reply));
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.TooManyRequests)
        {
            return StatusCode(
                StatusCodes.Status429TooManyRequests,
                "El asistente (DeepSeek/OpenRouter) tiene muchas solicitudes o alcanzó el límite gratis. Intenta de nuevo en unos minutos."
            );
        }
        catch (Exception)
        {
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                "Ocurrió un error al comunicarse con el asistente."
            );
        }
    }
}
