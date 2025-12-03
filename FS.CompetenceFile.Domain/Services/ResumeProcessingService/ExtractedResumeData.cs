namespace FS.CompetenceFile.Domain.Services.ResumeProcessingService;

/// <summary>
/// Represents structured data extracted from a resume PDF.
/// </summary>
public class ExtractedResumeData
{
    /// <summary>
    /// Gets or sets the candidate's full name.
    /// </summary>
    public string? FullName { get; set; }

    /// <summary>
    /// Gets or sets the candidate's email address.
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// Gets or sets the candidate's phone number.
    /// </summary>
    public string? Phone { get; set; }

    /// <summary>
    /// Gets or sets the candidate's address.
    /// </summary>
    public string? Address { get; set; }

    /// <summary>
    /// Gets or sets the candidate's professional summary or objective.
    /// </summary>
    public string? Summary { get; set; }

    /// <summary>
    /// Gets or sets the list of work experiences.
    /// </summary>
    public List<WorkExperience> WorkExperiences { get; set; } = new();

    /// <summary>
    /// Gets or sets the list of educational qualifications.
    /// </summary>
    public List<Education> Education { get; set; } = new();

    /// <summary>
    /// Gets or sets the list of skills.
    /// </summary>
    public List<string> Skills { get; set; } = new();

    /// <summary>
    /// Gets or sets the list of certifications.
    /// </summary>
    public List<Certification> Certifications { get; set; } = new();

    /// <summary>
    /// Gets or sets the list of languages and proficiency levels.
    /// </summary>
    public List<Language> Languages { get; set; } = new();

    /// <summary>
    /// Gets or sets any additional information extracted from the resume.
    /// </summary>
    public Dictionary<string, string> AdditionalInfo { get; set; } = new();
}
