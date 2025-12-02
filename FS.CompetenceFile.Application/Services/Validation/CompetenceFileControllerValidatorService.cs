using System.Reflection;
using FluentValidation;
using FluentValidation.Results;
using FS.CompetenceFile.Application.Interfaces.Validation;
using FS.CompetenceFile.Application.Validators;
using FS.CompetenceFile.Domain.Interfaces;
using FS.Framework.Reflection.Extensions;

namespace FS.CompetenceFile.Application.Services.Validation;

/// <inheritdoc />
public class CompetenceFileControllerValidatorServiceService : ICompetenceFileControllerValidatorService
{
    private readonly IValidator<ProcessResumeRequestValidator> _resumeRequestValidator;
    private readonly IValidator<ProcessResumeBatchRequestValidator> _processResumeBatchRequestValidator;
    private readonly IValidator<UploadTemplateRequestValidator> _uploadTemplateRequestValidator;

    /// <summary>
    /// Default constructor.
    /// </summary>
    /// <param name="resumeRequestValidator"></param>
    /// <param name="processResumeBatchRequestValidator"></param>
    /// <param name="uploadTemplateRequestValidator"></param>
    public CompetenceFileControllerValidatorServiceService(
        IValidator<ProcessResumeRequestValidator> resumeRequestValidator,
        IValidator<ProcessResumeBatchRequestValidator> processResumeBatchRequestValidator,
        IValidator<UploadTemplateRequestValidator> uploadTemplateRequestValidator)
    {
        _resumeRequestValidator = resumeRequestValidator;
        _processResumeBatchRequestValidator = processResumeBatchRequestValidator;
        _uploadTemplateRequestValidator = uploadTemplateRequestValidator;
    }

    /// <inheritdoc />
    public async ValueTask<ValidationResult> Validate(IFileRequest request)
    {
        var validator = SelectValidator(request);

        return await Task.Run(() =>
            validator?.GetType()?.GetMethod(nameof(validator.ValidateAsync))?.Invoke(validator, [request]) as
                ValidationResult ?? new ValidationResult
            {
                Errors =
                    [new ValidationFailure { ErrorMessage = "No validators found." }]
            });
    }

    private IValidator? SelectValidator(IFileRequest request)
    {
        var validatorField = this.GetFields()?.GetFieldByGenericArgument(request.GetType());
         
        return (this.GetFieldValue(validatorField, validatorField?.FieldType) ?? null) as IValidator;
    }
}