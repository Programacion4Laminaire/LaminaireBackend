namespace SharedKernel.Abstractions.Services;

public interface IChatbotService
{
    Task<string> GetReplyAsync(string userMessage, CancellationToken cancellationToken = default);
}
