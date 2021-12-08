using ChatTogether.FluentValidation.Rules;
using ChatTogether.ViewModels.Security;
using FluentValidation;

namespace ChatTogether.FluentValidation.Validators.Security
{
    public class RegistraionModelValidator : AbstractValidator<RegistrationModel>
    {
        public RegistraionModelValidator()
        {
            RuleFor(x => x.Email)
                .EmailAddress().WithMessage("Nieprawidłowy format adresu email.");

            RuleFor(x => x.Password)
                .MinimumLength(6).WithMessage("Hasło musi zawierać więcej niż 6 znaków.")
                .UpperChar(1).WithMessage("Hasło musi zawierać przynajmniej jedną wielką literę.")
                .DigitChar(1).WithMessage("Hasło musi zawierać przynajmniej jedną cyfrę.")
                .SpecialChar(1).WithMessage("Hasło musi zawierać przynajmniej jeden znak specjalny.");
        }
    }
}
