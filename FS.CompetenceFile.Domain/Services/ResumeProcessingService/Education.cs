namespace FS.CompetenceFile.Domain.Services.ResumeProcessingService;

/// <summary>
/// Represents an education entry.
/// </summary>
public class Education
{
    /// <summary>
    /// Gets or sets the educational institution name.
    /// </summary>
    public string? Institution { get; set; }

    /// <summary>
    /// Gets or sets the degree or qualification obtained.
    /// </summary>
    public string? Degree { get; set; }

    /// <summary>
    /// Gets or sets the field of study.
    /// </summary>
    public string? FieldOfStudy { get; set; }

    /// <summary>
    /// Gets or sets the graduation year.
    /// </summary>
    public string? Year { get; set; }
}

