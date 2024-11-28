using MediatR;
using Persistencia;
using Modelo.entidades;
using Aplicacion.Core;
using FluentValidation;
namespace Aplicacion.Transportistas.TransportistaCreate;

public class TransportistaCreateCommand
{
    public record TransportistaCreateCommandRequest(TransportistaCreateRequest transportistaCreateRequest) : IRequest<Result<int>>;

    internal class TransportistaCreateHandler : IRequestHandler<TransportistaCreateCommandRequest, Result<int>>
    {
        private readonly BackendContext _backendContext;
        public TransportistaCreateHandler(BackendContext backendContext)
        {
            _backendContext = backendContext;
        }
        public async Task<Result<int>> Handle(TransportistaCreateCommandRequest request, CancellationToken cancellationToken)
        {
            var transportista = new Transportista
            {
                Descripcion = request.transportistaCreateRequest.Descripcion,
                Tarifa = request.transportistaCreateRequest.Tarifa
            };
            
            _backendContext.Add(transportista);
            var resultado = await _backendContext.SaveChangesAsync(cancellationToken)> 0;
            return resultado 
                        ? Result<int>.Success(transportista.TransportistaID)
                        : Result<int>.Failure("No se pudo insertar el transportista.");
        }
    }

    public class TransportistaCreateCommandRequestValidator:AbstractValidator<TransportistaCreateCommandRequest>
    {
        public TransportistaCreateCommandRequestValidator()
        {
            RuleFor(x=>x.transportistaCreateRequest).SetValidator(new TransportistaCreateValidator());
        }
    }
}
