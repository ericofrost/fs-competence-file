namespace FS.CompetenceFile.Domain.Services.ResumeProcessingService;

/// <summary>
/// Represents a language and proficiency level.
/// </summary>
public class Language
{
    /// <summary>
    /// Gets or sets the language name.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the proficiency level (e.g., "Native", "Fluent", "Intermediate", "Basic").
    /// </summary>
    public string? Proficiency { get; set; }
}

