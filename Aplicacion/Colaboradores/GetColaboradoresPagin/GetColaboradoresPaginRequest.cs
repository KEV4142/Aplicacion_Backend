using Aplicacion.Core;

namespace Aplicacion.Colaboradores.GetColaboradoresPagin;
public class GetColaboradoresPaginRequest : PagingParams
{
    public string? Nombre { get; set; }
    public string? Direccion { get; set; }
    public string? Estado { get; set; }
}
