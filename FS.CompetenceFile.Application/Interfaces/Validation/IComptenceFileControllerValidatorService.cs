using FluentValidation.Results;
using FS.CompetenceFile.Domain.Interfaces;

namespace FS.CompetenceFile.Application.Interfaces.Validation;

/// <summary>
/// Provides functionality for validating competence file-related requests in the controller implementations.
/// </summary>
public interface ICompetenceFileControllerValidatorService
{
    /// <summary>
    /// Asynchronously validates the provided file request and returns the validation results.
    /// </summary>
    /// <param name="request">The file request to be validated, derived from the IBaseFileRequest interface.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the validation result
    /// indicating whether the request is valid or contains validation errors.
    /// </returns>
    ValueTask<ValidationResult> Validate(IFileRequest request);
}