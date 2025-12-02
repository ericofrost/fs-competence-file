using FS.CompetenceFile.Domain.Interfaces;
using Microsoft.AspNetCore.Http;

namespace FS.CompetenceFile.Domain.Models;

public class ProcessResumeBatchRequest : IBaseFileBatchRequest
{
    public IEnumerable<IFormFile>? Files { get; set; }
}