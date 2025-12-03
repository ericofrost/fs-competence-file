using FS.CompetenceFile.Domain.Interfaces;
using Microsoft.AspNetCore.Http;

namespace FS.CompetenceFile.Domain.Models;

/// <summary>
/// Represents a request to process a résumé.
/// </summary>
public class ProcessResumeRequest : IBaseFileRequest
{
    /// <summary>
    /// Gets or sets the PDF resume file to be processed.
    /// </summary>
    public IFormFile? File { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the template to be used for populating the resume data.
    /// </summary>
    public string TemplateId { get; set; } = string.Empty;
}