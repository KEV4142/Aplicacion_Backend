using Aplicacion.Core;

namespace Aplicacion.Transportistas.GetTransportistasPagin;
public class GetTransportistasPaginRequest : PagingParams
{
    public string? Descripcion { get; set; }
}
