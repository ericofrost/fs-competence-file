using FluentValidation;
using FS.CompetenceFile.Domain.Models;
using Microsoft.AspNetCore.Http;

namespace FS.CompetenceFile.Application.Validators;

/// <summary>
/// Validator class responsible for validating <see cref="ProcessResumeRequest"/> objects.
/// Ensures that the uploaded file adheres to specific rules, such as file size and file type.
/// </summary>
public class ProcessResumeRequestValidator : BaseFileValidator<ProcessResumeRequest>
{
    public ProcessResumeRequestValidator()
    {
        RuleFor(x => x.File)
            .NotNull()
            .WithMessage("File is required.")
            .Must(file => file?.Length > 0)
            .WithMessage("File cannot be empty.")
            .Must(file => file?.Length <= MaxFileSizeInMb * 1024 * 1024)
            .WithMessage($"File size cannot exceed {MaxFileSizeInMb}MB.")
            .Must(HaveValidExtension)
            .WithMessage($"File must be a PDF document. Allowed extensions: {string.Join(", ", _allowedExtensions)}");
    }
}

