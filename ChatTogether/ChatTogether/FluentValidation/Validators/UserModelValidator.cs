using ChatTogether.ViewModels;
using FluentValidation;

namespace ChatTogether.FluentValidation.Validators
{
    public class UserModelValidator : AbstractValidator<UserViewModel>
    {
        public UserModelValidator()
        {
            RuleFor(x => x.Nickname)
                .MaximumLength(100).WithMessage("Maksymalnie 100 znaków.");

            RuleFor(x => x.FirstName)
                .MaximumLength(100).WithMessage("Maksymalnie 100 znaków.");

            RuleFor(x => x.LastName)
                .MaximumLength(100).WithMessage("Maksymalnie 100 znaków.");

            RuleFor(x => x.City)
                .MaximumLength(100).WithMessage("Maksymalnie 100 znaków.");

            RuleFor(x => x.Description)
                .MaximumLength(600).WithMessage("Maksymalnie 600 znaków.");
        }
    }
}
