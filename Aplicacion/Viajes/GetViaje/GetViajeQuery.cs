

namespace Aplicacion.Viajes.GetViaje;
public record ViajeResponse(
    int? ViajeID,
    DateTime? Fecha,
    int? SucursalID,
    string? UsuarioID,
    string? Estado
){
    public ViajeResponse() : this(null, null, null, null,null)
    {
    }
}