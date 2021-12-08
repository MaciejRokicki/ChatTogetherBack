using FluentValidation;
using FluentValidation.Validators;
using System.Linq;

namespace ChatTogether.FluentValidation.Rules
{
    public class DigitChar<T, TProperty> : PropertyValidator<T, TProperty>
    {
        public override string Name => "DigitCharRule";
        private int _min;

        public DigitChar(int min)
        {
            _min = min;
        }

        public override bool IsValid(ValidationContext<T> context, TProperty value)
        {
            string str = value as string;
            int digitCharCount = str.Count(x => char.IsDigit(x));

            if (digitCharCount >= _min)
                return true;

            context.MessageFormatter.AppendArgument("MinDigitChars", _min);
            return false;
        }
    }

    public static class DigitCharExtension
    {
        public static IRuleBuilderOptions<T, TProperty> DigitChar<T, TProperty>(this IRuleBuilder<T, TProperty> ruleBuilder, int min)
        {
            return ruleBuilder.SetValidator(new DigitChar<T, TProperty>(min));
        }
    }
}
