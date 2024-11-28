using FluentValidation;

namespace Aplicacion.Accounts.Login;
public class LoginValidator : AbstractValidator<LoginRequest>
{
    public LoginValidator()
    {
        // RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("El correo electrónico es obligatorio.")
            .EmailAddress().WithMessage("El formato del correo electrónico no es válido.");

        RuleFor(x => x.Password).NotEmpty().WithMessage("Campo Contraseña es Obligatorio.");
    }
}
