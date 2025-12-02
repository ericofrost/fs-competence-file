using FluentValidation.Results;
using FS.CompetenceFile.Application.Interfaces.Validation;
using FS.CompetenceFile.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace FS.CompetenceFile.Api.Controllers;

public abstract class BaseController : ControllerBase
{
    protected readonly ILogger<CompetenceFileController> Logger;
    protected readonly ICompetenceFileControllerValidatorService Validator;
    protected readonly string UploadDirectory;

    protected BaseController(ILogger<CompetenceFileController> logger, ICompetenceFileControllerValidatorService validator)
    {
        Logger = logger;
        Validator = validator;
        
        // Define a directory for uploads.
        // In a real application, you'd configure this from appsettings.json
        // and likely use a more robust storage solution (e.g., Azure Blob Storage, AWS S3).
        UploadDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");

        // Ensure the upload directory exists
        if (!Directory.Exists(UploadDirectory))
        {
            Directory.CreateDirectory(UploadDirectory);
        }
    }

    /// <summary>
    /// Handles the validation errors and returns a bad request response containing the validation error details.
    /// </summary>
    /// <param name="validationResult">The validation result containing details of validation errors.</param>
    /// <returns>A BadRequestObjectResult containing information about the validation errors.</returns>
    protected BadRequestObjectResult TreatValidationAsync(ValidationResult validationResult)
    {
        return BadRequest(new { status = "Validation Error", message = validationResult.Errors.Select(x => x.ErrorMessage) });
    }
}