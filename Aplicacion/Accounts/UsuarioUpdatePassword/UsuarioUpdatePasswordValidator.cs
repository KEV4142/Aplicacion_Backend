using FluentValidation;

namespace Aplicacion.Accounts.UsuarioUpdatePassword;
public class UsuarioUpdatePasswordValidator : AbstractValidator<UsuarioUpdatePasswordRequest>
{
    public UsuarioUpdatePasswordValidator()
    {        
        RuleFor(x => x.Password)
        .Cascade(CascadeMode.Stop)
        .NotEmpty().WithMessage("El campo password esta en blanco.")
        .Matches(@"[A-Z]").WithMessage("El campo password debe contener al menos una letra mayúscula.")
        .Matches(@"[\W_]").WithMessage("El campo password debe contener al menos un carácter especial.");
    }
}
