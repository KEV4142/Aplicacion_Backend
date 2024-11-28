using Aplicacion.Core;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistencia;

namespace Aplicacion.Transportistas.TransportistaUpdate;
public class TransportistaUpdateCommand
{
    public record TransportistaUpdateCommandRequest(TransportistaUpdateRequest TransportistaUpdateRequest, int TransportistaID):IRequest<Result<int>>;

    internal class TransportistaUpdateCommandHandler
    : IRequestHandler<TransportistaUpdateCommandRequest, Result<int>>
    {
        private readonly BackendContext _context;

        public TransportistaUpdateCommandHandler(BackendContext context)
        {
            _context = context;
        }

        public async Task<Result<int>> Handle(
            TransportistaUpdateCommandRequest request, 
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

            transportista.Descripcion = request.TransportistaUpdateRequest.Descripcion;
            transportista.Tarifa = request.TransportistaUpdateRequest.Tarifa;
            transportista.Estado = request.TransportistaUpdateRequest.Estado;

            _context.Entry(transportista).State = EntityState.Modified;
            var resultado = await _context.SaveChangesAsync() > 0;

            return resultado 
                        ? Result<int>.Success(transportista.TransportistaID)
                        : Result<int>.Failure("Errores en el update del Transportista");

        }
    }


    public class TransportistaUpdateCommandRequestValidator
    : AbstractValidator<TransportistaUpdateCommandRequest>
    {
        public TransportistaUpdateCommandRequestValidator()
        {
            RuleFor(x => x.TransportistaUpdateRequest).SetValidator(new TransportistaUpdateValidator());
            RuleFor(x => x.TransportistaID).NotNull();
        }
    }    
}
