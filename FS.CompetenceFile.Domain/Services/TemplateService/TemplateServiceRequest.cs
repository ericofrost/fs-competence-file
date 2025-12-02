using Microsoft.AspNetCore.Http;

namespace FS.CompetenceFile.Domain.Services.TemplateService;

/// <summary>
/// Represents a service request for handling template-related operations in the TemplateService.
/// </summary>
public class TemplateServiceRequest
{
    /// <summary>
    /// Gets or sets the file associated with the template service request.
    /// This represents the file sent as part of the request to handle template-related operations,
    /// and it is expected to be of type <see cref="IFormFile"/>.
    /// </summary>
    public IFormFile? File { get; set; }
}