using Aplicacion.Core;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistencia;

namespace Aplicacion.SucursalesColaboradores.GetSucursalColaboradores;
public class GetSucursalColaboradores
{
    public record GetSucursalColaboradoresQueryRequest : IRequest<Result<List<SucursalesColaboradoresResponse>>>
    {
        public int SucursalID { get; set; }
    }
    internal class GetSucursalColaboradoresQueryHandler
    : IRequestHandler<GetSucursalColaboradoresQueryRequest, Result<List<SucursalesColaboradoresResponse>>>
    {
        private readonly BackendContext _context;
        private readonly IMapper _mapper;

        public GetSucursalColaboradoresQueryHandler(BackendContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result<List<SucursalesColaboradoresResponse>>> Handle(
            GetSucursalColaboradoresQueryRequest request, 
            CancellationToken cancellationToken
        )
        {
            var sucursalColaboradores = await _context.SucursalesColaboradores!.Where(x => x.SucursalID == request.SucursalID)
            .ProjectTo<SucursalesColaboradoresResponse>(_mapper.ConfigurationProvider)
            .ToListAsync();
            return Result<List<SucursalesColaboradoresResponse>>.Success(sucursalColaboradores!);
        }
    }
}
public record SucursalesColaboradoresResponse(
    int SucursalID,
    int ColaboradorID,
    int Distancia,
    string Estado
);