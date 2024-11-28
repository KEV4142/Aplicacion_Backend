using System.ComponentModel.DataAnnotations;

namespace Modelo.entidades
{
    public partial class Colaborador
    {
        public int ColaboradorID { get; set; }

        public string Nombre { get; set; } = null!;

        public string Direccion { get; set; } = null!;

        public string Coordenada { get; set; } = null!;

        public string Estado { get; set; } = null!;

        public virtual ICollection<SucursalColaborador> SucursalesColaboradores { get; set; } = new List<SucursalColaborador>();

        public virtual ICollection<ViajeDetalle> ViajesDetalles { get; set; } = new List<ViajeDetalle>();
    }
}
