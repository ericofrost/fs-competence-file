using FS.CompetenceFile.Domain.Services.FileSystemService;
using Microsoft.AspNetCore.Http;

namespace FS.CompetenceFile.Application.Interfaces.FileSystem;

public interface IFileSystemService
{
    ValueTask<FileSystemServiceResult> SaveFileAsync(IFormFile file, string path);
}