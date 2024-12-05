namespace Aplicacion.Accounts.UsuarioCreate;
public class UsuarioCreateRequest
{
    public string? NombreCompleto { get; set; }
    public string? Username { get; set; }

    public string? Email { get; set; }

    public string? Tipo { get; set; }

    public string? Password { get; set; }
}
