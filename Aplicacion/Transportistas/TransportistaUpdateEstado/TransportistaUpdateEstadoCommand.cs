using Aplicacion.Core;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistencia;

namespace Aplicacion.Transportistas.TransportistaUpdateEstado;
public class TransportistaUpdateEstadoCommand
{
    public record TransportistaUpdateEstadoCommandRequest(TransportistaUpdateEstadoRequest TransportistaUpdateEstadoRequest, int TransportistaID):IRequest<Result<int>>;

    internal class TransportistaUpdateEstadoCommandHandler
    : IRequestHandler<TransportistaUpdateEstadoCommandRequest, Result<int>>
    {
        private readonly BackendContext _context;

        public TransportistaUpdateEstadoCommandHandler(BackendContext context)
        {
            _context = context;
        }

        public async Task<Result<int>> Handle(
            TransportistaUpdateEstadoCommandRequest request, 
            CancellationToken cancellationToken
        )
        {
            var transportistaID = request.TransportistaID;

            var transportista = await _context.Transportistas!
            .FirstOrDefaultAsync(x => x.TransportistaID == transportistaID);
            
            if (transportista is null)
            {
                return Result<int>.Failure("El Transportista no existe");
            }

            transportista.Estado = request.TransportistaUpdateEstadoRequest.Estado!.ToUpper();

            _context.Entry(transportista).State = EntityState.Modified;
            var resultado = await _context.SaveChangesAsync() > 0;

            return resultado 
                        ? Result<int>.Success(transportista.TransportistaID)
                        : Result<int>.Failure("Errores al Actualizar Estado del Transportista");

        }
    }

    public class TransportistaUpdateEstadoCommandRequestValidator
    : AbstractValidator<TransportistaUpdateEstadoCommandRequest>
    {
        public TransportistaUpdateEstadoCommandRequestValidator()
        {
            RuleFor(x => x.TransportistaUpdateEstadoRequest).SetValidator(new TransportistaUpdateEstadoValidator());
            RuleFor(x => x.TransportistaID).NotNull();
        }
    } 
}