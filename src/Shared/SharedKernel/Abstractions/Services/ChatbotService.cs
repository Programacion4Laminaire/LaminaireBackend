using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using SharedKernel.Abstractions.Services;

namespace Identity.Api.Services;

public class ChatbotService : IChatbotService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public ChatbotService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }

    public async Task<string> GetReplyAsync(string userMessage, CancellationToken cancellationToken = default)
    {
        var apiKey = _configuration["OpenAI:ApiKey"];
        var model = _configuration["OpenAI:Model"] ?? "gpt-4o-mini";

        if (string.IsNullOrWhiteSpace(apiKey))
        {
            throw new InvalidOperationException("OpenAI:ApiKey no está configurado.");
        }

        using var request = new HttpRequestMessage(
            HttpMethod.Post,
            "https://api.openai.com/v1/chat/completions" // endpoint clásico de chat
        );

        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

        var body = new
        {
            model,
            messages = new[]
            {
                new { role = "system", content = "Eres un asistente que ayuda a los usuarios a usar el sistema SIR de Laminaire. Responde de forma clara y corta." },
                new { role = "user",   content = userMessage }
            }
        };

        var json = JsonSerializer.Serialize(body);
        request.Content = new StringContent(json, Encoding.UTF8, "application/json");

        using var response = await _httpClient.SendAsync(request, cancellationToken);
        response.EnsureSuccessStatusCode();

        var responseJson = await response.Content.ReadAsStringAsync(cancellationToken);

        using var doc = JsonDocument.Parse(responseJson);
        var root = doc.RootElement;

        var content = root
            .GetProperty("choices")[0]
            .GetProperty("message")
            .GetProperty("content")
            .GetString();

        return content ?? string.Empty;
    }
}
