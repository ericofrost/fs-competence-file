using FS.CompetenceFile.Application.Interfaces.FileSystem;
using FS.CompetenceFile.Domain.Services;
using FS.CompetenceFile.Domain.Services.FileSystemService;
using FS.Framework.Helpers.Extensions.Files;
using Microsoft.AspNetCore.Http;

namespace FS.CompetenceFile.Infrastructure.FileSystem;

/// <summary>
/// Provides file system operations for handling file uploads and storage.
/// Implements <see cref="IFileSystemService"/> to offer functionalities such as saving files to a specified path.
/// </summary>
public class FileSystemService : IFileSystemService
{
    /// <summary>
    /// Saves the provided file to the specified path asynchronously.
    /// </summary>
    /// <param name="file">The file to be saved, represented as an instance of <see cref="IFormFile"/>.</param>
    /// <param name="path">The target path where the file should be saved.</param>
    /// <returns>A <see cref="FileSystemServiceResult"/> indicating the result of the file saving operation,
    /// including success or error state, and additional contextual information.</returns>
    public async ValueTask<FileSystemServiceResult> SaveFileAsync(IFormFile file, string path)
    {
        var result = new FileSystemServiceResult( );
        
        try
        {
            var filePath = file.FilePath(path);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }
        }
        catch (Exception ex)
        {
            result.OutputType = OutputType.ERROR;
            result.Message = ex.Message;
        }

        return result;
    }
}