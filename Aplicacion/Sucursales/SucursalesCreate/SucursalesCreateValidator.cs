using FluentValidation;

namespace Aplicacion.Sucursales.SucursalesCreate;
public class SucursalesCreateValidator : AbstractValidator<SucursalesCreateRequest>
{
    public SucursalesCreateValidator()
    {
        RuleFor(x => x.Descripcion).NotEmpty().WithMessage("La Descripcion esta en blanco.");
        RuleFor(x => x.Direccion).NotEmpty().WithMessage("La Direccion esta en blanco.");
        RuleFor(x => x.Coordenada).NotEmpty().WithMessage("La Coordenada esta en blanco.");
    }
}

