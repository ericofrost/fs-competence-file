namespace FS.CompetenceFile.Domain.Services.ResumeProcessingService;

/// <summary>
/// Represents a certification.
/// </summary>
public class Certification
{
    /// <summary>
    /// Gets or sets the certification name.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the issuing organization.
    /// </summary>
    public string? IssuingOrganization { get; set; }

    /// <summary>
    /// Gets or sets the issue date.
    /// </summary>
    public string? IssueDate { get; set; }

    /// <summary>
    /// Gets or sets the expiration date, if applicable.
    /// </summary>
    public string? ExpirationDate { get; set; }
}

