using FluentValidation;

namespace Aplicacion.Transportistas.TransportistaCreate;

public class TransportistaCreateValidator:AbstractValidator<TransportistaCreateRequest>
{
    public TransportistaCreateValidator()
    {
        RuleFor(x=>x.Descripcion).NotEmpty().WithMessage("La Descripcion esta en blanco.");
        RuleFor(x=>x.Tarifa).GreaterThan(0).WithMessage("La Tarifa debe ser mayor a 0 o campo en blanco.");
    }

}
