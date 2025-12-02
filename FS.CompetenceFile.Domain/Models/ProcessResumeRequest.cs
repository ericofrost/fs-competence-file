using FS.CompetenceFile.Domain.Interfaces;
using Microsoft.AspNetCore.Http;

namespace FS.CompetenceFile.Domain.Models;

/// <summary>
/// Represents a request to process a résumé.
/// </summary>
public class ProcessResumeRequest : IBaseFileRequest
{
    public IFormFile? File { get; set; }
}