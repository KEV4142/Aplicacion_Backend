using Aplicacion.Core;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistencia;

namespace Aplicacion.Sucursales.SucursalesUpdateEstado;
public class SucursalesUpdateEstadoCommand
{
    public record SucursalesUpdateEstadoCommandRequest(SucursalesUpdateEstadoRequest sucursalesUpdateEstadoRequest, int SucursalID):IRequest<Result<int>>;

    internal class SucursalesUpdateEstadoCommandHandler
    : IRequestHandler<SucursalesUpdateEstadoCommandRequest, Result<int>>
    {
        private readonly BackendContext _context;

        public SucursalesUpdateEstadoCommandHandler(BackendContext context)
        {
            _context = context;
        }

        public async Task<Result<int>> Handle(
            SucursalesUpdateEstadoCommandRequest request, 
            CancellationToken cancellationToken
        )
        {
            var sucursalID = request.SucursalID;

            var sucursal = await _context.Sucursales!
            .FirstOrDefaultAsync(x => x.SucursalID == sucursalID);
            
            if (sucursal is null)
            {
                return Result<int>.Failure("La Sucursal no existe");
            }

            sucursal.Estado = request.sucursalesUpdateEstadoRequest.Estado!.ToUpper();

            _context.Entry(sucursal).State = EntityState.Modified;
            var resultado = await _context.SaveChangesAsync() > 0;

            return resultado 
                        ? Result<int>.Success(sucursal.SucursalID)
                        : Result<int>.Failure("Errores al Actualizar Estado de la Sucursal.");

        }
    }

    public class SucursalesUpdateEstadoCommandRequestValidator
    : AbstractValidator<SucursalesUpdateEstadoCommandRequest>
    {
        public SucursalesUpdateEstadoCommandRequestValidator()
        {
            RuleFor(x => x.sucursalesUpdateEstadoRequest).SetValidator(new SucursalesUpdateEstadoValidator());
            RuleFor(x => x.SucursalID).NotNull();
        }
    } 
}
