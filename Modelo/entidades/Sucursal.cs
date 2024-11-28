
using System.ComponentModel.DataAnnotations;
namespace Modelo.entidades
{
    public class Sucursal
    {
        public int SucursalID { get; set; }

        public string Descripcion { get; set; } = null!;

        public string Direccion { get; set; } = null!;

        public string Coordenada { get; set; } = null!;

        public string Estado { get; set; } = null!;

        public virtual ICollection<SucursalColaborador> SucursalesColaboradores { get; set; } = new List<SucursalColaborador>();

        public virtual ICollection<Viaje> Viajes { get; set; } = new List<Viaje>();
    }

}