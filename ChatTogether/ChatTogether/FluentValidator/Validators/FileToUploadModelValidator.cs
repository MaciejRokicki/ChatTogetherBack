using ChatTogether.Commons.ConfigurationModels;
using ChatTogether.FluentValidator.Rules;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace ChatTogether.FluentValidator.Validators
{
    public class FileToUploadModelValidator : AbstractValidator<IFormFile>
    {
        public FileToUploadModelValidator(IOptions<StaticFilesConfiguration> staticFilesConfiguration)
        {
            RuleFor(x => x.ContentType)
                .FileType(staticFilesConfiguration.Value.AllowedFiles)
                .WithMessage($"Nieobslugiwany format pliku.");
        }
    }
}
