using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using FS.CompetenceFile.Application.Interfaces.ExternalServices;
using FS.CompetenceFile.Application.Interfaces.FileSystem;
using FS.CompetenceFile.Application.Interfaces.Services;
using FS.CompetenceFile.Domain.Services.ResumeProcessingService;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OpenAI;
using OpenAI.Chat;

namespace FS.CompetenceFile.Application.Services.ResumeProcessing;

/// <summary>
/// Implements resume data extraction using OpenAI GPT-4 Vision API.
/// This service extracts structured data from PDF resumes using AI.
/// </summary>
public class ResumeExtractionService : IResumeExtractionService
{
    private readonly ILogger<ResumeExtractionService> _logger;
    private readonly IConfiguration _configuration;
    private readonly IAiIntegrationClient _aiIntegrationClient;
    private readonly IFileSystemService _fileSystemService;

    /// <summary>
    /// Initializes a new instance of the <see cref="ResumeExtractionService"/> class.
    /// </summary>
    /// <param name="logger">The logger instance for logging operations.</param>
    /// <param name="configuration">The configuration instance for accessing settings.</param>
    /// <param name="aiIntegrationClient"></param>
    /// <param name="fileSystemService"></param>
    public ResumeExtractionService(ILogger<ResumeExtractionService> logger, IConfiguration configuration, IAiIntegrationClient aiIntegrationClient, IFileSystemService fileSystemService)
    {
        _logger = logger;
        _configuration = configuration;
        _aiIntegrationClient = aiIntegrationClient;
        _fileSystemService = fileSystemService;
    }

    /// <inheritdoc />
    public async ValueTask<ExtractedResumeData?> ExtractResumeDataAsync(IFormFile resumeFile)
    {
        try
        {
            _logger.LogInformation("Starting AI extraction for resume file: {FileName}", resumeFile.FileName);

            // Get OpenAI API key from configuration
            var apiKey = _configuration["OpenAI:ApiKey"];
            
            if (string.IsNullOrWhiteSpace(apiKey))
            {
                _logger.LogError("OpenAI API key is not configured");
                
                return null;
            }

            // Convert PDF to base64 for OpenAI Vision API
            var base64Pdf = await ToBase64(resumeFile);

            // Create the prompt for structured extraction
            var extractionPrompt = CreateExtractionPrompt();

            // Call OpenAI API
            var extractedData = await _aiIntegrationClient.SendAiPromptAsync(apiKey, base64Pdf, extractionPrompt);
            
            var serializedExtractedData = DeserializeExtractedData(extractedData);
    
            if (serializedExtractedData != null)
            {
                _logger.LogInformation("Successfully extracted resume data using OpenAI");
            }
            else
            {
                _logger.LogWarning("Failed to extract resume data using OpenAI");
            }

            return serializedExtractedData;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while extracting resume data using OpenAI");
            
            return null;
        }
    }

    private ExtractedResumeData? DeserializeExtractedData(string? jsonResponse)
    {
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
        
        return extractedData;
    }
    
    /// <summary>
    /// Creates a detailed prompt for extracting structured data from resumes.
    /// </summary>
    private static string CreateExtractionPrompt()
    {
        return """
               Extract all relevant information from this resume PDF and return it as a JSON object with the following structure:
               {
                 "FullName": "string",
                 "Email": "string",
                 "Phone": "string",
                 "Address": "string",
                 "Summary": "string",
                 "WorkExperiences": [
                   {
                     "Company": "string",
                     "Position": "string",
                     "StartDate": "string",
                     "EndDate": "string or null",
                     "Description": "string"
                   }
                 ],
                 "Education": [
                   {
                     "Institution": "string",
                     "Degree": "string",
                     "FieldOfStudy": "string",
                     "Year": "string"
                   }
                 ],
                 "Skills": ["string"],
                 "Certifications": [
                   {
                     "Name": "string",
                     "IssuingOrganization": "string",
                     "IssueDate": "string",
                     "ExpirationDate": "string or null"
                   }
                 ],
                 "Languages": [
                   {
                     "Name": "string",
                     "Proficiency": "string"
                   }
                 ],
                 "AdditionalInfo": {}
               }

               Extract all available information. If a field is not found, use null or empty string. Return ONLY the JSON object, no additional text.
               """;
    }
    
    private static async ValueTask<string> ToBase64(IFormFile file)
    {
        // Read PDF content
        byte[] pdfBytes;
            
        using (var memoryStream = new MemoryStream())
        {
            await file.CopyToAsync(memoryStream);
            pdfBytes = memoryStream.ToArray();
        }

        // Convert PDF to base64 for OpenAI Vision API
        var base64Pdf = Convert.ToBase64String(pdfBytes);
        
        return base64Pdf;
    }
}