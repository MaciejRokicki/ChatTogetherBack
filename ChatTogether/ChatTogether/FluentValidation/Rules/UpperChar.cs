using FluentValidation;
using FluentValidation.Validators;
using System.Linq;

namespace ChatTogether.FluentValidation.Rules
{
    public class UpperChar<T, TProperty> : PropertyValidator<T, TProperty>
    {
        public override string Name => "UpperCharRule";
        private int _min;

        public UpperChar(int min)
        {
            _min = min;
        }

        public override bool IsValid(ValidationContext<T> context, TProperty value)
        {
            string str = value as string;
            int upperCharCount = str.Count(x => char.IsUpper(x));

            if (upperCharCount >= _min)
                return true;

            context.MessageFormatter.AppendArgument("MinUpperChars", _min);
            return false;
        }
    }

    public static class UpperCharExtension
    {
        public static IRuleBuilderOptions<T, TProperty> UpperChar<T, TProperty>(this IRuleBuilder<T, TProperty> ruleBuilder, int min)
        {
            return ruleBuilder.SetValidator(new UpperChar<T, TProperty>(min));
        }
    }
}
