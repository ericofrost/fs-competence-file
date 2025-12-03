using FS.CompetenceFile.Application.Interfaces.Services;
using FS.CompetenceFile.Domain.Services;
using FS.CompetenceFile.Domain.Services.ResumeProcessingService;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace FS.CompetenceFile.Application.Services.ResumeProcessing;

/// <summary>
/// Provides resume processing functionality including PDF extraction, AI-powered data extraction, and Word template population.
/// Implements <see cref="IResumeProcessingService"/> to offer functionalities for processing resumes and populating templates.
/// </summary>
public class ResumeProcessingService : IResumeProcessingService
{
    private readonly ILogger<ResumeProcessingService> _logger;
    private readonly IResumeExtractionService _resumeExtractionService;
    private readonly ITemplatePopulationService _templatePopulationService;

    /// <summary>
    /// Initializes a new instance of the <see cref="ResumeProcessingService"/> class.
    /// </summary>
    /// <param name="logger">The logger instance for logging operations.</param>
    /// <param name="resumeExtractionService">The service for extracting data from PDF resumes using AI.</param>
    /// <param name="templatePopulationService">The service for populating Word templates with extracted data.</param>
    public ResumeProcessingService(
        ILogger<ResumeProcessingService> logger,
        IResumeExtractionService resumeExtractionService,
        ITemplatePopulationService templatePopulationService)
    {
        _logger = logger;
        _resumeExtractionService = resumeExtractionService;
        _templatePopulationService = templatePopulationService;
    }

    /// <inheritdoc />
    public async ValueTask<ResumeProcessingServiceResult> ProcessResumeAndPopulateTemplate(ResumeProcessingServiceRequest request)
    {
        var result = new ResumeProcessingServiceResult();

        try
        {   
            #region Validation            
            // Validate input
            if (request.ResumeFile == null || request.ResumeFile.Length == 0)
            {
                result.OutputType = OutputType.ERROR;
                result.Message = "Resume file is required and cannot be empty.";
                return result;
            }

            if (string.IsNullOrWhiteSpace(request.TemplateId))
            {
                result.OutputType = OutputType.ERROR;
                result.Message = "Template ID is required.";
                return result;
            }

            if (string.IsNullOrWhiteSpace(request.TemplatePath) || !File.Exists(request.TemplatePath))
            {
                result.OutputType = OutputType.ERROR;
                result.Message = $"Template file not found at path: {request.TemplatePath}";
                return result;
            }
            #endregion Validation
            
            _logger.LogInformation(
                "Starting resume processing for file '{FileName}' with template ID '{TemplateId}'",
                request.ResumeFile.FileName, request.TemplateId);

            // Step 1: Extract data from PDF resume using AI
            _logger.LogInformation("Extracting data from PDF resume using AI...");
            
            var extractedData = await _resumeExtractionService.ExtractResumeDataAsync(request.ResumeFile);

            if (extractedData == null)
            {
                result.OutputType = OutputType.ERROR;
                result.Message = "Failed to extract data from the resume. The resume may be unreadable or in an unsupported format.";
                return result;
            }

            _logger.LogInformation("Successfully extracted data from resume. Name: {Name}, Email: {Email}", extractedData.FullName, extractedData.Email);

            // Step 2: Populate the Word template with extracted data
            _logger.LogInformation("Populating Word template with extracted data...");
            
            var populatedTemplatePath = await _templatePopulationService.PopulateTemplateAsync(request.TemplatePath, extractedData, request.TemplateId);

            if (string.IsNullOrWhiteSpace(populatedTemplatePath) || !File.Exists(populatedTemplatePath))
            {
                result.OutputType = OutputType.ERROR;
                result.Message = "Failed to populate the template. The template may be corrupted or have invalid placeholders.";
                return result;
            }

            _logger.LogInformation("Successfully populated template. Output file: {PopulatedTemplatePath}", populatedTemplatePath);

            // Step 3: Return a success result
            result.OutputType = OutputType.SUCCESS;
            result.Message = "Resume processed and template populated successfully.";
            result.PopulatedTemplatePath = populatedTemplatePath;
            result.ExtractedData = extractedData;

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while processing resume and populating template");
            result.OutputType = OutputType.ERROR;
            result.Message = $"An error occurred during resume processing: {ex.Message}";
            return result;
        }
    }
}



/// <summary>
/// Interface for populating Word templates with extracted resume data.
/// This abstraction allows for different Word manipulation libraries to be used.
/// </summary>
public interface ITemplatePopulationService
{
    /// <summary>
    /// Populates a Word template with extracted resume data.
    /// </summary>
    /// <param name="templatePath">The path to the Word template file.</param>
    /// <param name="extractedData">The extracted resume data to populate the template with.</param>
    /// <param name="templateId">The template ID for logging/tracking purposes.</param>
    /// <returns>The path to the populated template file, or null if population fails.</returns>
    ValueTask<string?> PopulateTemplateAsync(string templatePath, ExtractedResumeData extractedData, string templateId);
}

