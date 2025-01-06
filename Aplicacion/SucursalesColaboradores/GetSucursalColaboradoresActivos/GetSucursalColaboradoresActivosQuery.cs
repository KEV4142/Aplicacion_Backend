using Aplicacion.Core;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistencia;

namespace Aplicacion.SucursalesColaboradores.GetSucursalColaboradoresActivos;
public class GetSucursalColaboradoresActivos
{
    public record GetSucursalColaboradoresActivosQueryRequest : IRequest<Result<List<SucursalColaboradoresActivosResponse>>>
    {
        public int SucursalID { get; set; }
    }
    internal class GetSucursalColaboradoresActivosQueryHandler
    : IRequestHandler<GetSucursalColaboradoresActivosQueryRequest, Result<List<SucursalColaboradoresActivosResponse>>>
    {
        private readonly BackendContext _context;
        private readonly IMapper _mapper;

        public GetSucursalColaboradoresActivosQueryHandler(BackendContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result<List<SucursalColaboradoresActivosResponse>>> Handle(
            GetSucursalColaboradoresActivosQueryRequest request,
            CancellationToken cancellationToken
        )
        {
            var sucursalColaboradores = await _context.SucursalesColaboradores!
            .Include(sc => sc.Colaborador)
            .Include(sc => sc.Sucursal)
            .Where(x => x.SucursalID == request.SucursalID && x.Estado.ToUpper() == "A")
            .Select(x => new SucursalColaboradoresActivosResponse(
                x.SucursalID,
                x.Sucursal.Descripcion,
                x.ColaboradorID,
                Funciones.ToProperCase(x.Colaborador.Nombre),
                x.Distancia,
                x.Estado
            )).ToListAsync();
            return Result<List<SucursalColaboradoresActivosResponse>>.Success(sucursalColaboradores!);
        }
    }
}
public record SucursalColaboradoresActivosResponse(
    int SucursalID,
    string Descripcion,
    int ColaboradorID,
    string Nombre,
    int Distancia,
    string Estado
);
