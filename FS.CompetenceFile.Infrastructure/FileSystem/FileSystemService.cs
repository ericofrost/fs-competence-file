using System.Text;
using FS.CompetenceFile.Application.Interfaces.FileSystem;
using FS.CompetenceFile.Domain.Services;
using FS.CompetenceFile.Domain.Services.FileSystemService;
using FS.Framework.Helpers.Extensions.Files;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace FS.CompetenceFile.Infrastructure.FileSystem;

/// <summary>
/// Provides file system operations for handling file uploads and storage.
/// Implements <see cref="IFileSystemService"/> to offer functionalities such as saving files to a specified path.
/// </summary>
public class FileSystemService(ILogger<FileSystemService> logger) : IFileSystemService
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

    public async ValueTask<string> ExtractTextFromPdfAsync(string base64Pdf)
    {
        return await Task.Run(() =>
        {
            try
            {
                var pdfBytes = Convert.FromBase64String(base64Pdf);

                using var stream = new MemoryStream(pdfBytes);
                using var reader = new iText.Kernel.Pdf.PdfReader(stream);
                using var pdfDocument = new iText.Kernel.Pdf.PdfDocument(reader);
                var textBuilder = new StringBuilder();

                for (int i = 1; i <= pdfDocument.GetNumberOfPages(); i++)
                {
                    var page = pdfDocument.GetPage(i);
                    var strategy = new iText.Kernel.Pdf.Canvas.Parser.Listener.LocationTextExtractionStrategy();
                    var processor = new iText.Kernel.Pdf.Canvas.Parser.PdfCanvasProcessor(strategy);
                    processor.ProcessPageContent(page);
                    var pageText = strategy.GetResultantText();
                    if (!string.IsNullOrWhiteSpace(pageText))
                    {
                        textBuilder.AppendLine(pageText);
                    }
                }

                return textBuilder.ToString();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error extracting text from PDF");
                return string.Empty;
            }
        });
    }
}