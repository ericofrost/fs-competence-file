using FS.CompetenceFile.Domain.Services.TemplateService;

namespace FS.CompetenceFile.Application.Interfaces.Services;

/// <summary>
/// Provides functionalities to handle operations related to template processing.
/// </summary>
public interface ITemplateService
{
    /// <summary>
    /// Processes a template upload request and returns the result of the operation.
    /// </summary>
    /// <param name="request">The <see cref="TemplateServiceRequest"/> containing the template file to be processed.</param>
    /// <returns>A <see cref="TemplateServiceResult"/> indicating the outcome of the template processing.</returns>
    ValueTask<TemplateServiceResult> ProcessUploadTemplate(TemplateServiceRequest request);
}