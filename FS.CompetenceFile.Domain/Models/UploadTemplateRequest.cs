using FS.CompetenceFile.Domain.Interfaces;
using Microsoft.AspNetCore.Http;

namespace FS.CompetenceFile.Domain.Models;

/// <summary>
/// Represents a request to upload a template.
/// </summary>
public class UploadTemplateRequest: IBaseFileRequest
{
    public IFormFile? File { get; set; }
}
