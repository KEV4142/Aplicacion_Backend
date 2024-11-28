using Modelo;
using Microsoft.EntityFrameworkCore;
using Modelo.entidades;

namespace Persistencia
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("¡Hola, mundo!");


            using var context = new BackendContext();
            var Nuevo = new Sucursal
            {
                Descripcion = "Megamall",
                Direccion = "Mall MegaMall",
                Coordenada="100.2"
            };

            var cursos = await context.Sucursales!.ToListAsync();
            foreach (var curso in cursos)
            {
                Console.WriteLine($"{curso.SucursalID}   {curso.Descripcion}");
            }
            /* context.Add(Nuevo);
            await context.SaveChangesAsync(); */
            var cursos2 = await context.Sucursales!.ToListAsync();
            foreach (var curso in cursos2)
            {
                // Console.WriteLine($"{curso.SucursalID}   {curso.Descripcion}");
                Console.WriteLine($"{curso.SucursalesColaboradores}");
            }
        }
    }
}