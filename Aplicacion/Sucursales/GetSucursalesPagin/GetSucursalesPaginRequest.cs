using Aplicacion.Core;

namespace Aplicacion.Sucursales.GetSucursalesPagin;
public class GetSucursalesPaginRequest:PagingParams
{
    public string? Descripcion { get; set; }
    public string? Direccion { get; set; }
    public string? Estado { get; set; }
}
