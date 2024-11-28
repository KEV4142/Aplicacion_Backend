using Aplicacion.Core;
using Aplicacion.SucursalesColaboradores.GetSucursalColaboradores;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistencia;

namespace Aplicacion.SucursalesColaboradores.GetSucursalesColaboradoresActivas;
public class GetSucursalesColaboradoresActivas
{
    public record GetSucursalesColaboradoresActivasQueryRequest : IRequest<Result<List<SucursalesColaboradoresResponse>>>
    {
    }
    internal class GetSucursalesColaboradoresActivasQueryHandler
        : IRequestHandler<GetSucursalesColaboradoresActivasQueryRequest, Result<List<SucursalesColaboradoresResponse>>>
    {
        private readonly BackendContext _context;
        private readonly IMapper _mapper;

        public GetSucursalesColaboradoresActivasQueryHandler(BackendContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result<List<SucursalesColaboradoresResponse>>> Handle(
            GetSucursalesColaboradoresActivasQueryRequest request, 
            CancellationToken cancellationToken
        )
        {
            var sucursalesColaboradores = await _context.SucursalesColaboradores!
                .Where(sc => sc.Estado!=null && sc.Estado.ToUpper() =="A")
                .ProjectTo<SucursalesColaboradoresResponse>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return Result<List<SucursalesColaboradoresResponse>>.Success(sucursalesColaboradores);
        }
    }
}
