using System.Text.Json;
using System.Text.Json.Serialization;
using FS.CompetenceFile.Application.Interfaces.ExternalServices;
using FS.CompetenceFile.Application.Interfaces.FileSystem;
using FS.CompetenceFile.Domain.Services.ResumeProcessingService;
using Microsoft.Extensions.Logging;
using OpenAI;
using OpenAI.Chat;

namespace FS.CompetenceFile.Application.Services.ExternalServices;

public class OpenAiIntegrationClient : IAiIntegrationClient
{
    private readonly ILogger<OpenAiIntegrationClient> _logger;
    private readonly IFileSystemService _fileSystemService;

    public OpenAiIntegrationClient(ILogger<OpenAiIntegrationClient> logger, IFileSystemService fileSystemService)
    {
        _logger = logger;
        _fileSystemService = fileSystemService;
    }
    
    public async ValueTask<string?> SendAiPromptAsync(string apiKey, string base64Pdf, string prompt)
    {
        try
        {
            // Initialize the OpenAI client
            var openAiClient = new OpenAIClient(apiKey);

            // Get the chat client for the specified model
            var chatClient = openAiClient.GetChatClient("gpt-4o");

            // Extract text from PDF first (for better accuracy, we can also use vision API with images)
            var pdfText = await _fileSystemService.ExtractTextFromPdfAsync(base64Pdf);

            // Create chat completion with structured output
            var completion = await chatClient.CompleteChatAsync(
                [
                    new SystemChatMessage(
                        "You are an expert at extracting structured data from resumes. Return only valid JSON."),
                    new UserChatMessage($"{prompt}\n\nResume Content:\n{pdfText}")
                ],
                new ChatCompletionOptions
                {
                    Temperature = 0.1f, // Lower temperature for more consistent extraction
                    ResponseFormat = ChatResponseFormat.CreateJsonObjectFormat() // Request JSON object format
                });

            // Get the response content
            var jsonResponse = completion?.Value.Content[0].Text;

            if (string.IsNullOrWhiteSpace(jsonResponse))
            {
                _logger.LogWarning("OpenAI returned empty response");
                return null;
            }

            // Parse the JSON response
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };

            var extractedData = JsonSerializer.Deserialize<ExtractedResumeData>(jsonResponse, options);

            if (extractedData == null)
            {
                _logger.LogWarning("Failed to deserialize extracted resume data from JSON response");
                return null;
            }

            return jsonResponse;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calling OpenAI API");
            return null;
        }
    }
}