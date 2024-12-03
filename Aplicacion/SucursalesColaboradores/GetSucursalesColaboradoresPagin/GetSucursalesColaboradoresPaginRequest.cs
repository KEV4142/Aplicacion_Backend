using Aplicacion.Core;

namespace Aplicacion.SucursalesColaboradores.GetSucursalesColaboradoresPagin;
public class GetSucursalesColaboradoresPaginRequest : PagingParams
{
    public int? SucursalID { get; set; }
    public int? ColaboradorID { get; set; }
    public string? Estado { get; set; }
}
