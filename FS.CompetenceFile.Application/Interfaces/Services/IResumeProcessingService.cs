using FS.CompetenceFile.Domain.Services.ResumeProcessingService;

namespace FS.CompetenceFile.Application.Interfaces.Services;

/// <summary>
/// Provides functionalities to handle operations related to resume processing and template population.
/// </summary>
public interface IResumeProcessingService
{
    /// <summary>
    /// Processes a resume PDF file, extracts information using AI, and populates a Word template with the extracted data.
    /// </summary>
    /// <param name="request">The <see cref="ResumeProcessingServiceRequest"/> containing the resume file and template information.</param>
    /// <returns>A <see cref="ResumeProcessingServiceResult"/> indicating the outcome of the resume processing operation, including the path to the populated template.</returns>
    ValueTask<ResumeProcessingServiceResult> ProcessResumeAndPopulateTemplate(ResumeProcessingServiceRequest request);
}

