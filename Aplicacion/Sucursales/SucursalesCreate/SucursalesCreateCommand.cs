using Aplicacion.Core;
using FluentValidation;
using MediatR;
using Modelo.entidades;
using Persistencia;

namespace Aplicacion.Sucursales.SucursalesCreate;
public class SucursalesCreateCommand
{
    public record SucursalesCreateCommandRequest(SucursalesCreateRequest sucursalesCreateRequest) : IRequest<Result<int>>;

    internal class SucursalesCreateHandler : IRequestHandler<SucursalesCreateCommandRequest, Result<int>>
    {
        private readonly BackendContext _backendContext;
        public SucursalesCreateHandler(BackendContext backendContext)
        {
            _backendContext = backendContext;
        }
        public async Task<Result<int>> Handle(SucursalesCreateCommandRequest request, CancellationToken cancellationToken)
        {
            var sucursal = new Sucursal
            {
                Descripcion = request.sucursalesCreateRequest.Descripcion,
                Direccion = request.sucursalesCreateRequest.Direccion,
                Coordenada = request.sucursalesCreateRequest.Coordenada
            };
            
            _backendContext.Add(sucursal);
            var resultado = await _backendContext.SaveChangesAsync(cancellationToken)> 0;
            return resultado 
                        ? Result<int>.Success(sucursal.SucursalID)
                        : Result<int>.Failure("No se pudo insertar la sucursal.");
        }
    }

    public class SucursalesCreateCommandRequestValidator:AbstractValidator<SucursalesCreateCommandRequest>
    {
        public SucursalesCreateCommandRequestValidator()
        {
            RuleFor(x=>x.sucursalesCreateRequest).SetValidator(new SucursalesCreateValidator());
        }
    }
}
