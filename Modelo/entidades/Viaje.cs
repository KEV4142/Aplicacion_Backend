namespace Modelo.entidades;

public class Viaje
{
    public int ViajeID { get; set; }

    public DateTime Fecha { get; set; }

    public int SucursalID { get; set; }

    public string? UsuarioID { get; set; }

    public int TransportistaID { get; set; }

    public string Estado { get; set; } = null!;

    public virtual Sucursal Sucursal { get; set; } = null!;

    public virtual Transportista Transportista { get; set; } = null!;
    // public virtual AppUser Usuario { get; set; }

    public virtual ICollection<ViajeDetalle> ViajesDetalles { get; set; } = new List<ViajeDetalle>();

}
