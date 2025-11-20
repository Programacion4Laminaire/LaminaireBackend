using SharedKernel.Abstractions.Services;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Identity.Api.Services;

public class OpenRouterChatbotService : IChatbotService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public OpenRouterChatbotService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }

    public async Task<string> GetReplyAsync(string userMessage, CancellationToken cancellationToken = default)
    {
        var apiKey = _configuration["OpenRouter:ApiKey"];
        var model = _configuration["OpenRouter:Model"] ?? "deepseek/deepseek-r1:free";
        var siteUrl = _configuration["OpenRouter:SiteUrl"] ?? "http://localhost";
        var appName = _configuration["OpenRouter:AppName"] ?? "Asistente SIR";

        if (string.IsNullOrWhiteSpace(apiKey))
            throw new InvalidOperationException("OpenRouter:ApiKey no está configurado.");

        using var request = new HttpRequestMessage(
            HttpMethod.Post,
            "https://openrouter.ai/api/v1/chat/completions"
        );

        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
        request.Headers.Add("HTTP-Referer", siteUrl);
        request.Headers.Add("X-Title", appName);

        var body = new
        {
            model,
            messages = new[]
            {
                new { role = "system", content = "Eres un asistente que ayuda a los usuarios a usar el sistema SIR de Laminaire. Responde de forma clara, corta y específica al contexto de gestión, pedidos, permisos y reportes internos." },
                new { role = "user",   content = userMessage }
            }
        };

        var json = JsonSerializer.Serialize(body);
        request.Content = new StringContent(json, Encoding.UTF8, "application/json");

        using var response = await _httpClient.SendAsync(request, cancellationToken);

        // Si OpenRouter devuelve 4xx/5xx, lanza HttpRequestException
        response.EnsureSuccessStatusCode();

        var responseJson = await response.Content.ReadAsStringAsync(cancellationToken);

        using var doc = JsonDocument.Parse(responseJson);
        var root = doc.RootElement;

        // Estructura compatible con OpenAI style: { choices: [ { message: { content: "..." } } ] }
        var content = root
            .GetProperty("choices")[0]
            .GetProperty("message")
            .GetProperty("content")
            .GetString();

        return content ?? string.Empty;
    }
}
