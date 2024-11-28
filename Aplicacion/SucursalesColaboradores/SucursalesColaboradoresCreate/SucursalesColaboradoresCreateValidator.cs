using FluentValidation;

namespace Aplicacion.SucursalesColaboradores.SucursalesColaboradoresCreate;
public class SucursalesColaboradoresCreateValidator:AbstractValidator<SucursalesColaboradoresCreateRequest>
{
    public SucursalesColaboradoresCreateValidator()
    {
        RuleFor(x=>x.SucursalID).GreaterThan(0).WithMessage("Se debe tener seleccionado una Sucursal.");
        RuleFor(x=>x.ColaboradorID).GreaterThan(0).WithMessage("Se debe de tener seleccionado un Colaborador.");
        RuleFor(x=>x.Distancia).InclusiveBetween(1,50).WithMessage("La Distancia debe ser en rango de 1 a 50.");
    }

}