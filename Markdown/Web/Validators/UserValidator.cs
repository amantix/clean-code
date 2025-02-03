using FluentValidation;
using Markdown.Requests;

namespace Markdown.Validators
{
    public class UserValidator : AbstractValidator<RegistrationRequest>
    {
        public UserValidator() {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress()
                .WithMessage("Неправильный формат почты");

            RuleFor(user => user.Username)
            .NotEmpty()
            .Matches("^[\\w\\d_\\\\s]{5,25}$")
            .WithMessage("Логин должен быть от 5 до 25 символов.");


            RuleFor(x => x.Password)
                .NotEmpty()
                .Matches(@"^(?=.*[A-Z])(?=.*\d)(?=.*[\@$!%*?&])[A-Za-z\d\D]{8,}$")
                .WithMessage("Пароль должен быть от 8 до 20 символов, " +
                         "иметь хотя бы одно число, специальный символ, и букву в верхнем регистре.");

            RuleFor(x => x.AnotherPassword)
                .NotEmpty()
                .Equal(x=>x.Password)
                .WithMessage("Пароли должны совпадать.");
        }
    }
}
