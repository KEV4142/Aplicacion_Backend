namespace Aplicacion.Viajes.ViajeCreate;
public class ViajeCreateRequest
{
    public DateTime Fecha { get; set; }

    public int SucursalID { get; set; }

    // public string? UsuarioID { get; set; }

    public int TransportistaID { get; set; }
    public List<ViajeDetalleRequest> ViajesDetalle { get; set; } = new List<ViajeDetalleRequest>();
    
}
public class ViajeDetalleRequest
{
    public int ColaboradorID { get; set; }
    // public string? UsuarioID { get; set; }
}