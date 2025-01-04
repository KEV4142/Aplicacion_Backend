namespace Aplicacion.Sucursales.SucursalesUpdate;
public class SucursalesUpdateRequest
{
    public string Descripcion { get; set; } = null!;
    public string Direccion { get; set; } = null!;
    public string Coordenada { get; set; } = null!;
    public string Estado { get; set; } = null!;
}