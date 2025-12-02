using FS.CompetenceFile.Application.Interfaces.Validation;
using FS.CompetenceFile.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace FS.CompetenceFile.Api.Controllers;

public class CompetenceFileController : BaseController
{
    /// <summary>
    /// Default constructor.
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="validator"> Controller Validator <see cref="ICompetenceFileControllerValidatorService"/></param>
    public CompetenceFileController(ILogger<CompetenceFileController> logger, ICompetenceFileControllerValidatorService validator) : base(logger, validator)
    {
    }

    /// <summary>
    /// Receives a .docx file as form data.
    /// </summary>
    /// <param name="request"></param>
    /// <returns>A status indicating success or failure.</returns>
    [HttpPost("UploadTemplate")]
    [RequestSizeLimit(5 * 1024 * 1024)] // Limit to 5 MB
    public async ValueTask<IActionResult> UploadTemplate([FromForm] UploadTemplateRequest request)
    {
        var validationResult = await Validator.Validate(request);
        
        if (!validationResult.IsValid)
        {
            return this.TreatValidationAsync(validationResult);
        }

        try
        {
            var fileExtension = Path.GetExtension(request.File.FileName).ToLowerInvariant();
            var uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";
            var filePath = Path.Combine(UploadDirectory, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await request.File.CopyToAsync(fileStream);
            }

            Logger.LogInformation($"UploadTemplate: Successfully uploaded '{request.File.FileName}' as '{uniqueFileName}'.");
            return Ok(new { status = "success", message = "Document template uploaded successfully." });
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, $"UploadTemplate: An error occurred while uploading '{request.File.FileName}'.");
            return StatusCode(StatusCodes.Status500InternalServerError,
                new { status = "error", message = $"An error occurred during file upload: {ex.Message}" });
        }
    }

    /// <summary>
    /// Receives a PDF file as form data and processes it.
    /// </summary>
    /// <param name="request">The uploaded PDF file.</param>
    /// <returns>A JSON object with resume ID, status, and an optional error message.</returns>
    [HttpPost("ProcessResume")]
    [RequestSizeLimit(10 * 1024 * 1024)] // Limit to 10 MB
    public async Task<IActionResult> ProcessResume([FromForm] ProcessResumeRequest request)
    {
        var validationResult = await Validator.Validate(request);
        
        if (!validationResult.IsValid)
        {
            return this.TreatValidationAsync(validationResult);
        }

        var allowedExtensions = new[] { ".pdf" };
        var fileExtension = Path.GetExtension(request.File.FileName).ToLowerInvariant();

        if (!allowedExtensions.Contains(fileExtension))
        {
            Logger.LogWarning($"ProcessResume: Invalid file extension '{fileExtension}'. Only .pdf is allowed.");
            return BadRequest(new CompetenceFileProcessResponse
            {
                ResumeId = Guid.Empty,
                Status = "error",
                ErrorMessage = "Invalid file type. Only PDF files are allowed for resumes."
            });
        }

        var resumeId = Guid.NewGuid();
        var uniqueFileName = $"{resumeId}{fileExtension}";
        var filePath = Path.Combine(UploadDirectory, uniqueFileName);

        try
        {
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await request.File.CopyToAsync(fileStream);
            }

            Logger.LogInformation(
                $"ProcessResume: Successfully received and saved '{request.File.FileName}' as '{uniqueFileName}'. Initiating processing for Resume ID: {resumeId}");

            // Simulate processing...
            await Task.Delay(TimeSpan.FromSeconds(2));

            var random = new Random();

            if (random.Next(0, 100) < 80)
            {
                Logger.LogInformation($"ProcessResume: Resume ID {resumeId} processed successfully.");
                return Ok(new CompetenceFileProcessResponse
                {
                    ResumeId = resumeId,
                    Status = "success",
                    ErrorMessage = null,
                    FileName = request.File.FileName
                });
            }
            else
            {
                var errorMessage = "Simulated processing failure: Could not extract all information from resume.";
                Logger.LogError($"ProcessResume: Resume ID {resumeId} processing failed. {errorMessage}");
                return Ok(new CompetenceFileProcessResponse
                {
                    ResumeId = resumeId,
                    Status = "error",
                    ErrorMessage = errorMessage,
                    FileName = request.File.FileName
                });
            }
        }
        catch (Exception ex)
        {
            Logger.LogError(ex,
                $"ProcessResume: An error occurred while processing resume '{request.File.FileName}' for ID {resumeId}.");
            return StatusCode(StatusCodes.Status500InternalServerError, new CompetenceFileProcessResponse
            {
                ResumeId = resumeId,
                Status = "error",
                ErrorMessage = $"An error occurred during resume processing: {ex.Message}",
                FileName = request.File.FileName
            });
        }
    }

    /// <summary>
    /// Receives multiple PDF files as form data and processes them in batch.
    /// </summary>
    /// <param name="request">The collection of PDF files to process.</param>
    /// <returns>A collection of processing results for each file.</returns>
    [HttpPost("ProcessResumeBatch")]
    [RequestSizeLimit(100 * 1024 * 1024)] // Limit to 100 MB for batch upload
    public async Task<IActionResult> ProcessResumeBatch([FromForm] ProcessResumeBatchRequest request)
    {
        var validationResult = await Validator.Validate(request);
        
        if (!validationResult.IsValid)
        {
            return this.TreatValidationAsync(validationResult);
        }

        var results = new List<CompetenceFileProcessResponse>();

        foreach (var file in request.Files)
        {
            if (file.Length == 0)
            {
                results.Add(new CompetenceFileProcessResponse
                {
                    ResumeId = Guid.Empty,
                    Status = "error",
                    ErrorMessage = $"File '{file.FileName}' is empty.",
                    FileName = file.FileName
                });
                continue;
            }

            var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
            
            if (fileExtension != ".pdf")
            {
                results.Add(new CompetenceFileProcessResponse
                {
                    ResumeId = Guid.Empty,
                    Status = "error",
                    ErrorMessage = $"File '{file.FileName}' is not a PDF file.",
                    FileName = file.FileName
                });
                continue;
            }

            var resumeId = Guid.NewGuid();
            var uniqueFileName = $"{resumeId}{fileExtension}";
            var filePath = Path.Combine(UploadDirectory, uniqueFileName);

            try
            {
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }

                Logger.LogInformation(
                    $"ProcessResumeBatch: Successfully received and saved '{file.FileName}' as '{uniqueFileName}'. Initiating processing for Resume ID: {resumeId}");

                // Simulate processing...
                await Task.Delay(TimeSpan.FromSeconds(2));

                var random = new Random();

                if (random.Next(0, 100) < 80)
                {
                    Logger.LogInformation($"ProcessResumeBatch: Resume ID {resumeId} processed successfully.");
                    results.Add(new CompetenceFileProcessResponse
                    {
                        ResumeId = resumeId,
                        Status = "success",
                        ErrorMessage = null,
                        FileName = file.FileName
                    });
                }
                else
                {
                    var errorMessage = "Simulated processing failure: Could not extract all information from resume.";
                    Logger.LogError($"ProcessResumeBatch: Resume ID {resumeId} processing failed. {errorMessage}");
                    results.Add(new CompetenceFileProcessResponse
                    {
                        ResumeId = resumeId,
                        Status = "error",
                        ErrorMessage = errorMessage,
                        FileName = file.FileName
                    });
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex,
                    $"ProcessResumeBatch: An error occurred while processing resume '{file.FileName}' for ID {resumeId}.");
                results.Add(new CompetenceFileProcessResponse
                {
                    ResumeId = resumeId,
                    Status = "error",
                    ErrorMessage = $"An error occurred during resume processing: {ex.Message}",
                    FileName = file.FileName
                });
            }
        }

        return Ok(new { Status = "completed", Results = results });
    }
}