using Aplicacion.Accounts.GetUsuariosPagin;
using Aplicacion.Colaboradores.GetColaborador;
using Aplicacion.Sucursales.GetSucursal;
using Aplicacion.SucursalesColaboradores.GetSucursalColaboradores;
using Aplicacion.SucursalesColaboradores.GetSucursalColaboradoresActivos;
using Aplicacion.Transportistas.GetTransportista;
using Aplicacion.Viajes.GetViaje;
using AutoMapper;
using Modelo.entidades;
using Persistencia.Models;

namespace Aplicacion.Core;
public class MappingProfile:Profile
{
    public MappingProfile()
    {
         CreateMap<Transportista, TransportistaResponse>();
         CreateMap<Sucursal, SucursalResponse>();
         CreateMap<SucursalColaborador, SucursalesColaboradoresResponse>();
         CreateMap<Colaborador, ColaboradorResponse>();
         CreateMap<Viaje, ViajeResponse>();

   
    }
   
}
