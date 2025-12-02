namespace FS.CompetenceFile.Domain.Services;

/// <summary>
/// Represents the base structure for an output result, including the type of the output,
/// an associated message, and a boolean indicator to specify if the operation should be aborted.
/// </summary>
public class BaseResult
{
    /// <summary>
    /// Gets or sets the type of output represented by this instance.
    /// The type is defined by the <c>OutputType</c> enumeration and specifies
    /// the nature of the output, such as ERROR, WARNING, INFO, or SUCCESS.
    /// </summary>
    public OutputType OutputType { get; set; } = OutputType.SUCCESS;

    /// <summary>
    /// Gets or sets the message associated with the output.
    /// The message provides additional information or context about the result or outcome of an operation.
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a value indicating whether the operation should be aborted.
    /// When set to <c>true</c>, it suggests that the process should terminate before completion.
    /// </summary>
    public bool ShouldAbort { get; set; }
}