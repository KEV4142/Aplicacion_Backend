using Aplicacion.Core;

namespace Aplicacion.Accounts.GetUsuariosPagin;
public class GetUsuariosPaginRequest : PagingParams
{
    public string? Email { get; set; }
    public string? Username { get; set; }
}
