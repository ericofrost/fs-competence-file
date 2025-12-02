namespace FS.CompetenceFile.Domain.Models;

/// <summary>
/// Data for returning after processing a Competence File.
/// </summary>
public class CompetenceFileProcessResponse
{
    /// <summary>
    /// The competence file ID.
    /// </summary>
    public Guid ResumeId { get; set; }

    /// <summary>
    /// The status of the file processing.
    /// </summary>
    public string Status { get; set; } = "Success"; // "success" or "error"

    /// <summary>
    /// Error message if the result is an error.
    /// </summary>
    public string? ErrorMessage { get; set; } // Optional error message

    /// <summary>
    /// The name of the uploaded file associated with the competence file processing.
    /// </summary>
    public string FileName { get; set; } = string.Empty;
}