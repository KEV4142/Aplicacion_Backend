
using FluentValidation;

namespace Aplicacion.Viajes.ViajeCreate;
public class ViajeCreateValidator : AbstractValidator<ViajeCreateRequest>
{
    public ViajeCreateValidator()
    {
        RuleFor(x => x.Fecha).NotEmpty().WithMessage("El campo Fecha esta en blanco.");
        RuleFor(x => x.SucursalID).GreaterThan(0).WithMessage("Se debe tener seleccionado una Sucursal.");
        RuleFor(x => x.TransportistaID).GreaterThan(0).WithMessage("Se debe de tener seleccionado un Transportista.");
        // RuleFor(x => x.UsuarioID).NotEmpty().WithMessage("El campo Usuario esta en blanco en el encabezado.");
        RuleForEach(x => x.ViajesDetalle).SetValidator(new ViajeDetalleValidator()).WithMessage("Error en los detalles del viaje.");
        RuleFor(x => x.ViajesDetalle).NotEmpty().WithMessage("La lista de detalles no puede estar vacía.");
    }

}
public class ViajeDetalleValidator : AbstractValidator<ViajeDetalleRequest>
{
    public ViajeDetalleValidator()
    {
        RuleFor(x => x.ColaboradorID).GreaterThan(0).WithMessage("Colaborador no valido o debe ser mayor que 0.");
        // RuleFor(x => x.UsuarioID).NotEmpty().WithMessage("El campo Usuario no puede estar vacío en el Detalle.");
    }
}