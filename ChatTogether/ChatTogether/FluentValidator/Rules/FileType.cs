using FluentValidation;
using FluentValidation.Validators;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace ChatTogether.FluentValidator.Rules
{
    public class FileType<T, TProperty> : PropertyValidator<T, TProperty>
    {
        public override string Name => "FileSizeRule";
        private string[] _types;

        public FileType(string[] types)
        {
            _types = types;
        }

        public override bool IsValid(ValidationContext<T> context, TProperty value)
        {
            string fileType = value as string;

            if (_types.Contains(fileType))
                return true;

            context.MessageFormatter.AppendArgument("FileSize", _types);
            return false;
        }
    }

    public static class FileTypeExtension
    {
        public static IRuleBuilderOptions<T, TProperty> FileType<T, TProperty>(this IRuleBuilder<T, TProperty> ruleBuilder, string[] types)
        {
            return ruleBuilder.SetValidator(new FileType<T, TProperty>(types));
        }
    }
}
