using Aplicacion.Core;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistencia;

namespace Aplicacion.Sucursales.SucursalesUpdate;
public class SucursalesUpdateCommand
{
    public record SucursalesUpdateCommandRequest(SucursalesUpdateRequest sucursalesUpdateRequest, int SucursalID):IRequest<Result<int>>;

    internal class SucursalesUpdateCommandHandler
    : IRequestHandler<SucursalesUpdateCommandRequest, Result<int>>
    {
        private readonly BackendContext _context;

        public SucursalesUpdateCommandHandler(BackendContext context)
        {
            _context = context;
        }

        public async Task<Result<int>> Handle(
            SucursalesUpdateCommandRequest request, 
            CancellationToken cancellationToken
        )
        {
            var sucursalID = request.SucursalID;

            var sucursal = await _context.Sucursales!
            .FirstOrDefaultAsync(x => x.SucursalID == sucursalID);
            
            if (sucursal is null)
            {
                return Result<int>.Failure("La Sucursal no existe.");
            }

            sucursal.Descripcion = request.sucursalesUpdateRequest.Descripcion;
            sucursal.Direccion = request.sucursalesUpdateRequest.Direccion;
            sucursal.Coordenada = request.sucursalesUpdateRequest.Coordenada;
            sucursal.Estado = request.sucursalesUpdateRequest.Estado;

            _context.Entry(sucursal).State = EntityState.Modified;
            var resultado = await _context.SaveChangesAsync() > 0;

            return resultado 
                        ? Result<int>.Success(sucursal.SucursalID)
                        : Result<int>.Failure("Errores en la Actualizacion de la Sucursal.");

        }
    }

    public class SucursalesUpdateCommandRequestValidator
    : AbstractValidator<SucursalesUpdateCommandRequest>
    {
        public SucursalesUpdateCommandRequestValidator()
        {
            RuleFor(x => x.sucursalesUpdateRequest).SetValidator(new SucursalesUpdateValidator());
            RuleFor(x => x.SucursalID).NotNull();
        }
    }  
}
