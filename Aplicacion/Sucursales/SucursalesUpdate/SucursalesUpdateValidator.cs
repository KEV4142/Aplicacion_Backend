using FluentValidation;

namespace Aplicacion.Sucursales.SucursalesUpdate;
public class SucursalesUpdateValidator : AbstractValidator<SucursalesUpdateRequest>
{
    public SucursalesUpdateValidator()
    {
        RuleFor(x => x.Descripcion).NotEmpty().WithMessage("La Descripcion esta en blanco.");
        RuleFor(x => x.Direccion).NotEmpty().WithMessage("La Direccion esta en blanco.");
        RuleFor(x => x.Coordenada).NotEmpty().WithMessage("La Coordenada esta en blanco.");
        RuleFor(x=>x.Estado)
        .Cascade(CascadeMode.Stop)
        .NotEmpty().WithMessage("El Estado no debe de estar en blanco.")
        .Must(estado => estado == "a" || estado == "b" || estado == "i" || estado == "A" || estado == "B" || estado == "I")
                .WithMessage("El Estado debe ser A, B o I.");
    }
}
