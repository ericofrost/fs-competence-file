namespace FS.CompetenceFile.Domain.Services.ResumeProcessingService;

/// <summary>
/// Represents a work experience entry.
/// </summary>
public class WorkExperience
{
    /// <summary>
    /// Gets or sets the company name.
    /// </summary>
    public string? Company { get; set; }

    /// <summary>
    /// Gets or sets the job title or position.
    /// </summary>
    public string? Position { get; set; }

    /// <summary>
    /// Gets or sets the start date of employment.
    /// </summary>
    public string? StartDate { get; set; }

    /// <summary>
    /// Gets or sets the end date of employment. Can be null if currently employed.
    /// </summary>
    public string? EndDate { get; set; }

    /// <summary>
    /// Gets or sets the job description or responsibilities.
    /// </summary>
    public string? Description { get; set; }
}

