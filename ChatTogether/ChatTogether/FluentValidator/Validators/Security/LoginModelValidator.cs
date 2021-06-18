using ChatTogether.FluentValidator.Rules;
using ChatTogether.ViewModels.Security;
using FluentValidation;

namespace ChatTogether.FluentValidator.Validators.Security
{
    public class LoginModelValidator : AbstractValidator<LoginModel>
    {
        public LoginModelValidator()
        {
            RuleFor(x => x.Email)
                .EmailAddress().WithMessage("Nieprawidłowy format adresu email.");

            RuleFor(x => x.Password)
                .MinimumLength(6).WithMessage("Hasło musie zawierać więcej niż 6 znaków.")
                .UpperChar(1).WithMessage("Hasło musi zawierać przynajmniej jedną wielką literę.")
                .DigitChar(1).WithMessage("Hasło musi zawierać przynajmniej jedną cyfrę.")
                .SpecialChar(1).WithMessage("Hasło musi zawierać przynajmniej jeden znak specjalny.");
        }
    }
}
