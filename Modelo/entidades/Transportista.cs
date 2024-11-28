namespace Modelo.entidades;

public class Transportista
{
    public int TransportistaID { get; set; }

    public string? Descripcion { get; set; }

    public decimal Tarifa { get; set; }

    public string? Estado { get; set; }

    public ICollection<Viaje>? Viajes { get; set; }

}
