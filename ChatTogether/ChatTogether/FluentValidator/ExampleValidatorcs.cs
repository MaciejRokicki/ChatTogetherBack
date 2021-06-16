using FluentValidation;
using ChatTogether.ViewModels;

namespace ChatTogether.FluentValidator
{
    public class ExampleValidator : AbstractValidator<ExampleViewModel>
    {
        public ExampleValidator()
        {
            RuleFor(x => x.Txt)
                .NotEmpty().WithMessage("Te pole nie może być puste.");
        }
    }
}
