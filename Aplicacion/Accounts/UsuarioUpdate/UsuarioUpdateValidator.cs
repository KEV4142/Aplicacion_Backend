using FluentValidation;

namespace Aplicacion.Accounts.UsuarioUpdate;
public class UsuarioUpdateValidator : AbstractValidator<UsuarioUpdateRequest>
{
    public UsuarioUpdateValidator()
    {
        RuleFor(x => x.Email)
        .Cascade(CascadeMode.Stop)
        .NotEmpty().WithMessage("El Campo Email esta vacio.")
        .EmailAddress().WithMessage("El Email ingresado no es formato correcto.");
        
        RuleFor(x => x.NombreCompleto).NotEmpty().WithMessage("El campo nombre se encuentra vacio.");
        
        RuleFor(x => x.Tipo).NotEmpty().WithMessage("El campo Tipo esta en blanco.");

        RuleFor(x => x.Username).NotEmpty().WithMessage("Ingrese un username.");
    }
}
