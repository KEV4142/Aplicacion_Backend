using FluentValidation;
namespace Aplicacion.Transportistas.TransportistaUpdate;
public class TransportistaUpdateValidator:AbstractValidator<TransportistaUpdateRequest>
{
    public TransportistaUpdateValidator()
    {
        RuleFor(x => x.Descripcion).NotEmpty().WithMessage("La Descripcion no debe estar en blanco.");
        RuleFor(x => x.Tarifa).GreaterThan(0).WithMessage("La Tarifa debe ser mayor a 0 o campo no debe de estar en blanco.");
        RuleFor(x=>x.Estado).NotEmpty().WithMessage("El Estado no debe de estar en blanco.");
    }

}
