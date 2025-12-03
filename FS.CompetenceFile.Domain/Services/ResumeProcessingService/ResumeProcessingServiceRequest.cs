using Microsoft.AspNetCore.Http;

namespace FS.CompetenceFile.Domain.Services.ResumeProcessingService;

/// <summary>
/// Represents a service request for processing a resume and populating a template.
/// </summary>
public class ResumeProcessingServiceRequest
{
    /// <summary>
    /// Gets or sets the PDF resume file to be processed.
    /// </summary>
    public IFormFile? ResumeFile { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the template to be used for populating the resume data.
    /// </summary>
    public string TemplateId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the path where the template file is stored.
    /// </summary>
    public string TemplatePath { get; set; } = string.Empty;
}