namespace Modelo.entidades;
public class ViajeDetalle
{
    public int ViaDetID { get; set; }

    public int ViajeID { get; set; }

    public int ColaboradorID { get; set; }

    public string? UsuarioID { get; set; }
    // public virtual AppUser Usuario { get; set; }

    public virtual Colaborador Colaborador { get; set; }  = null!;

    public virtual Viaje Viaje { get; set; } = null!;

}

