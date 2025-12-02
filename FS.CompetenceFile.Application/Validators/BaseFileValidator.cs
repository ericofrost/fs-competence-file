using FluentValidation;
using FS.CompetenceFile.Domain.Interfaces;
using Microsoft.AspNetCore.Http;

namespace FS.CompetenceFile.Application.Validators;

/// <summary>
/// Represents a base validator class designed to validate file request objects that implement the <see cref="IFileRequest"/> interface.
/// This class defines common validation logic and constants for file size and file extensions.
/// </summary>
/// <typeparam name="TRequest">
/// The type of the file request object to validate. This type must implement the <see cref="IFileRequest"/> interface.
/// </typeparam>
public abstract class BaseFileValidator<TRequest> : AbstractValidator<TRequest> where TRequest : IFileRequest
{
    protected const int MaxFileSizeInMb = 5;
    protected readonly string[] _allowedExtensions = [".docx"];

    /// <summary>
    /// Determines whether the provided file has a valid extension based on the allowed extensions.
    /// </summary>
    /// <param name="file">The file to validate for valid extensions. Null files will return false.</param>
    /// <returns>True if the file has a valid extension; otherwise, false.</returns>
    protected bool HaveValidExtension(IFormFile? file)
    {
        if (file == null) return false;
        
        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        
        return _allowedExtensions.Contains(extension);
    }
}