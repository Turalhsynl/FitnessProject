namespace Application.Abstractions;

public interface IOpenAIService
{
     Task<string> GetResponseAsync(string userMessage);
}
