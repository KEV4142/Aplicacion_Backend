using System.ComponentModel.DataAnnotations;

namespace Modelo.entidades
{
    public class SucursalColaborador
    {
        public int SucursalID { get; set; }

        public int ColaboradorID { get; set; }

        public int Distancia { get; set; }

        public string Estado { get; set; } = null!;

        public virtual Colaborador Colaborador { get; set; } = null!;

        public virtual Sucursal Sucursal { get; set; } = null!;
    }
}

