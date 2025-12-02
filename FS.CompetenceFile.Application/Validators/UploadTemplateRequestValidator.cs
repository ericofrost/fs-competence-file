using FluentValidation;
using FS.CompetenceFile.Domain.Models;
using Microsoft.AspNetCore.Http;

namespace FS.CompetenceFile.Application.Validators;

public class UploadTemplateRequestValidator : BaseFileValidator<UploadTemplateRequest>
{
    public UploadTemplateRequestValidator()
    {
        RuleFor(x => x.File)
            .NotNull()
            .WithMessage("File is required.")
            .Must(file => file?.Length > 0)
            .WithMessage("File cannot be empty.")
            .Must(file => file?.Length <= MaxFileSizeInMb * 1024 * 1024)
            .WithMessage($"File size cannot exceed {MaxFileSizeInMb}MB.")
            .Must(HaveValidExtension)
            .WithMessage($"File must be a DOCX document. Allowed extensions: {string.Join(", ", _allowedExtensions)}");
    }
}