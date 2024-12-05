using FluentValidation;

namespace Aplicacion.Accounts.UsuarioCreate;
public class UsuarioCreateValidator : AbstractValidator<UsuarioCreateRequest>
{
    public UsuarioCreateValidator()
    {
        RuleFor(x => x.Email)
        .Cascade(CascadeMode.Stop)
        .NotEmpty().WithMessage("El Campo Email esta vacio.")
        .EmailAddress().WithMessage("El Email ingresado no es formato correcto.");
        
        RuleFor(x => x.Password)
        .Cascade(CascadeMode.Stop)
        .NotEmpty().WithMessage("El campo password esta en blanco.")
        .Matches(@"[A-Z]").WithMessage("El campo password debe contener al menos una letra mayúscula.")
        .Matches(@"[\W_]").WithMessage("El campo password debe contener al menos un carácter especial.");
        
        RuleFor(x => x.NombreCompleto).NotEmpty().WithMessage("El campo nombre se encuentra vacio.");
        
        RuleFor(x => x.Tipo).NotEmpty().WithMessage("El campo Tipo esta en blanco.");

        RuleFor(x => x.Username).NotEmpty().WithMessage("Ingrese un username.");
    }
}
