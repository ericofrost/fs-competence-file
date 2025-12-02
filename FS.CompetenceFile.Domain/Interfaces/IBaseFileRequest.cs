using Microsoft.AspNetCore.Http;

namespace FS.CompetenceFile.Domain.Interfaces;

/// <summary>
/// Represents a base interface for file requests.
/// </summary>
public interface IBaseFileRequest : IFileRequest
{
    /// <summary>
    /// Represents a file associated with the request.
    /// </summary>
    /// <remarks>
    /// This property allows access to the file being transferred within the request context.
    /// </remarks>
    IFormFile? File { get; set; }
}