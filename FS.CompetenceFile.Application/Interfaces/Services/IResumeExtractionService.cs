using FS.CompetenceFile.Domain.Services.ResumeProcessingService;
using Microsoft.AspNetCore.Http;

namespace FS.CompetenceFile.Application.Interfaces.Services;

/// <summary>
/// Interface for extracting structured data from PDF resumes using AI.
/// This abstraction allows for different AI providers to be used.
/// </summary>
public interface IResumeExtractionService
{
    /// <summary>
    /// Extracts structured data from a PDF resume file using AI.
    /// </summary>
    /// <param name="resumeFile">The PDF resume file to extract data from.</param>
    /// <returns>The extracted resume data, or null if extraction fails.</returns>
    ValueTask<ExtractedResumeData?> ExtractResumeDataAsync(IFormFile resumeFile);
}