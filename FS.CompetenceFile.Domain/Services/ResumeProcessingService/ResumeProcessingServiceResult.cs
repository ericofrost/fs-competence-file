using FS.CompetenceFile.Domain.Services;

namespace FS.CompetenceFile.Domain.Services.ResumeProcessingService;

/// <summary>
/// Represents the result of a resume processing operation.
/// Inherits from <see cref="BaseResult"/> to provide additional context about the operation's outcome.
/// </summary>
public class ResumeProcessingServiceResult : BaseResult
{
    /// <summary>
    /// Gets or sets the path to the populated template file.
    /// This will be set when the operation is successful.
    /// </summary>
    public string? PopulatedTemplatePath { get; set; }

    /// <summary>
    /// Gets or sets the extracted resume data in a structured format.
    /// This contains the information extracted from the PDF résumé.
    /// </summary>
    public ExtractedResumeData? ExtractedData { get; set; }
}

