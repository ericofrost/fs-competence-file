namespace FS.CompetenceFile.Application.Interfaces.ExternalServices;

public interface IAiIntegrationClient
{
    ValueTask<string?> SendAiPromptAsync(string apiKey, string base64Pdf, string prompt);
}