using FluentValidation;
using FluentValidation.Validators;
using System.Linq;

namespace ChatTogether.FluentValidator.Rules
{
    public class SpecialChar<T, TProperty> : PropertyValidator<T, TProperty>
    {
        public override string Name => "SpecialCharRule";
        private int _min;

        public SpecialChar(int min)
        {
            _min = min;
        }

        public override bool IsValid(ValidationContext<T> context, TProperty value)
        {
            string str = value as string;
            int specialCharCount = str.Length - str.Count(char.IsLetterOrDigit);

            if (specialCharCount >= _min)
                return true;

            context.MessageFormatter.AppendArgument("MinSpecialChars", _min);
            return false;
        }
    }

    public static class SpecialCharExtension
    {
        public static IRuleBuilderOptions<T, TProperty> SpecialChar<T, TProperty>(this IRuleBuilder<T, TProperty> ruleBuilder, int min)
        {
            return ruleBuilder.SetValidator(new SpecialChar<T, TProperty>(min));
        }
    }
}
