using Aplicacion.Core;
using Aplicacion.Interfaces;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Modelo.entidades;
using Persistencia;
using Persistencia.Models;

namespace Aplicacion.Viajes.ViajeCreate;
public class ViajeCreateCommand
{
    public record ViajeCreateCommandRequest(ViajeCreateRequest viajeCreateRequest) : IRequest<Result<int>>;

    internal class ViajeCreateHandler : IRequestHandler<ViajeCreateCommandRequest, Result<int>>
    {
        private readonly BackendContext _backendContext;
        private readonly UserManager<AppUser> _userManager;
        private readonly IUserAccessor _user;
        public ViajeCreateHandler(BackendContext backendContext, UserManager<AppUser> userManager, IUserAccessor user)
        {
            _backendContext = backendContext;
            _userManager = userManager;
            _user = user;
        }
        public async Task<Result<int>> Handle(ViajeCreateCommandRequest request, CancellationToken cancellationToken)
        {
            var usuarioID = _user.GetUserId();
            if (string.IsNullOrEmpty(usuarioID))
            {
                return Result<int>.Failure("No se encontró el UsuarioID en la Autorizacion.");
            }
            var viaje = new Viaje
            {
                Fecha = request.viajeCreateRequest.Fecha,
                SucursalID = request.viajeCreateRequest.SucursalID,
                UsuarioID = usuarioID,
                TransportistaID = request.viajeCreateRequest.TransportistaID
            };

            if (request.viajeCreateRequest.SucursalID > 0)
            {
                var sucursal = await _backendContext.Sucursales!
                .FirstOrDefaultAsync(x => x.SucursalID == request.viajeCreateRequest.SucursalID);

                if (sucursal is null)
                {
                    return Result<int>.Failure("No se encontro la Sucursal.");
                }

                viaje.Sucursal = sucursal;
            }

            if (request.viajeCreateRequest.TransportistaID > 0)
            {
                var transportista = await _backendContext.Transportistas!
                .FirstOrDefaultAsync(x => x.TransportistaID == request.viajeCreateRequest.TransportistaID);

                if (transportista is null)
                {
                    return Result<int>.Failure("No se encontro el Transportista.");
                }

                viaje.Transportista = transportista;
            }

            if (usuarioID is not null)
            {
                var appUsuario = await _userManager.Users!
                .FirstOrDefaultAsync(x => x.Id == usuarioID);
                if (appUsuario is null)
                {
                    return Result<int>.Failure("No se encontro el Usuario.");
                }
            }

            var viajeDetalles = new List<ViajeDetalle>();
            int suma = 0;
            foreach (var detalle in request.viajeCreateRequest.ViajesDetalle)
            {
                var colaborador = await _backendContext.Colaboradores!
                    .FirstOrDefaultAsync(c => c.ColaboradorID == detalle.ColaboradorID);
                if (colaborador is null)
                {
                    return Result<int>.Failure($"El Colaborador con ID {detalle.ColaboradorID} no es válido.");
                }

                var sucursalColaborador = await _backendContext.SucursalesColaboradores!
                .FirstOrDefaultAsync(sc => sc.SucursalID == request.viajeCreateRequest.SucursalID && sc.ColaboradorID == detalle.ColaboradorID);
                if (sucursalColaborador is null)
                {
                    return Result<int>.Failure($"El Colaborador con ID {detalle.ColaboradorID} no pertence a la Sucursal {viaje.Sucursal.Descripcion}.");
                }
                suma += sucursalColaborador.Distancia;

                var numeroViajesColaborador = await _backendContext.ViajesDetalles
                .Where(vd => vd.ColaboradorID == detalle.ColaboradorID &&
                    vd.Viaje.Fecha.Date == request.viajeCreateRequest.Fecha.Date)
                .CountAsync();

                if (numeroViajesColaborador > 0)
                {
                    return Result<int>.Failure($"El Colaborador con ID {detalle.ColaboradorID} ya ha viajado anteriormente en este dia({request.viajeCreateRequest.Fecha.Date.ToString("dd-MM-yyyy")}).");
                }
                if (suma > 100)
                {
                    return Result<int>.Failure("No procede viaje por mas de 100 KM.");
                }

                viajeDetalles.Add(new ViajeDetalle
                {
                    ColaboradorID = detalle.ColaboradorID,
                    UsuarioID = usuarioID,
                    Viaje = viaje
                });
            }

            viaje.ViajesDetalles = viajeDetalles;

            _backendContext.Add(viaje);
            var resultado = await _backendContext.SaveChangesAsync(cancellationToken) > 0;
            return resultado
                        ? Result<int>.Success(viaje.ViajeID)
                        : Result<int>.Failure("No se pudo insertar el registro del Viaje con su Detalle.");
        }
    }



    public class ViajeCreateCommandRequestValidator : AbstractValidator<ViajeCreateCommandRequest>
    {
        public ViajeCreateCommandRequestValidator()
        {
            RuleFor(x => x.viajeCreateRequest).SetValidator(new ViajeCreateValidator());
        }
    }

}