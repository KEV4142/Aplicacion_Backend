using Aplicacion.Core;
using FluentValidation;
using MediatR;
using Modelo.entidades;
using Persistencia;

namespace Aplicacion.SucursalesColaboradores.SucursalesColaboradoresCreate;
public class SucursalesColaboradoresCreateCommand
{
    public record SucursalesColaboradoresCreateCommandRequest(SucursalesColaboradoresCreateRequest sucursalesColaboradoresCreateRequest) : IRequest<Result<int>>;

    internal class SucursalesColaboradoresCreateHandler : IRequestHandler<SucursalesColaboradoresCreateCommandRequest, Result<int>>
    {
        private readonly BackendContext _backendContext;
        public SucursalesColaboradoresCreateHandler(BackendContext backendContext)
        {
            _backendContext = backendContext;
        }
        public async Task<Result<int>> Handle(SucursalesColaboradoresCreateCommandRequest request, CancellationToken cancellationToken)
        {
            var sucursalColaborador = new SucursalColaborador
            {
                SucursalID = request.sucursalesColaboradoresCreateRequest.SucursalID,
                ColaboradorID = request.sucursalesColaboradoresCreateRequest.ColaboradorID,
                Distancia = request.sucursalesColaboradoresCreateRequest.Distancia
            };

            if (request.sucursalesColaboradoresCreateRequest.SucursalID > 0)
            {
                var sucursal = _backendContext.Sucursales!
                .FirstOrDefault(x => x.SucursalID == request.sucursalesColaboradoresCreateRequest.SucursalID);

                if (sucursal is null)
                {
                    return Result<int>.Failure("No se encontro la Sucursal.");
                }

                sucursalColaborador.Sucursal = sucursal;
            }

            if (request.sucursalesColaboradoresCreateRequest.ColaboradorID > 0)
            {
                var colaborador = _backendContext.Colaboradores!
                .FirstOrDefault(x => x.ColaboradorID == request.sucursalesColaboradoresCreateRequest.ColaboradorID);

                if (colaborador is null)
                {
                    return Result<int>.Failure("No se encontro el Colaborador.");
                }

                sucursalColaborador.Colaborador = colaborador;
            }

            var existeRegistro = _backendContext.SucursalesColaboradores!
                                    .FirstOrDefault(x => x.SucursalID == sucursalColaborador.SucursalID &&
                                        x.ColaboradorID == sucursalColaborador.ColaboradorID
                                    );
            if (existeRegistro is not null)
            {
                return Result<int>.Failure($"Ya se tiene Registro del Colaborador(ID: {sucursalColaborador.ColaboradorID}) en la Sucursal(ID: {sucursalColaborador.SucursalID}).");
            }

            _backendContext.Add(sucursalColaborador);
            var resultado = await _backendContext.SaveChangesAsync(cancellationToken) > 0;
            return resultado
                        ? Result<int>.Success(sucursalColaborador.SucursalID)
                        : Result<int>.Failure("No se pudo insertar el registro del Colaborador con la Sucursal.");
        }
    }
    public class SucursalesColaboradoresCreateCommandRequestValidator : AbstractValidator<SucursalesColaboradoresCreateCommandRequest>
    {
        public SucursalesColaboradoresCreateCommandRequestValidator()
        {
            RuleFor(x => x.sucursalesColaboradoresCreateRequest).SetValidator(new SucursalesColaboradoresCreateValidator());
        }
    }
}