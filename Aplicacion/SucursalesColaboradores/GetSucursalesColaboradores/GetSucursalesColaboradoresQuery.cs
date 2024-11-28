using Aplicacion.Core;
using Aplicacion.SucursalesColaboradores.GetSucursalColaboradores;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistencia;

namespace Aplicacion.SucursalesColaboradores.GetSucursalesColaboradores;
public class GetSucursalesColaboradores
{
    public record GetSucursalesColaboradoresQueryRequest : IRequest<Result<List<SucursalesColaboradoresResponse>>>
    {
    }
    internal class GetSucursalesColaboradoresQueryHandler
        : IRequestHandler<GetSucursalesColaboradoresQueryRequest, Result<List<SucursalesColaboradoresResponse>>>
    {
        private readonly BackendContext _context;
        private readonly IMapper _mapper;

        public GetSucursalesColaboradoresQueryHandler(BackendContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result<List<SucursalesColaboradoresResponse>>> Handle(
            GetSucursalesColaboradoresQueryRequest request, 
            CancellationToken cancellationToken
        )
        {
            var sucursalesColaboradores = await _context.SucursalesColaboradores!
                .ProjectTo<SucursalesColaboradoresResponse>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return Result<List<SucursalesColaboradoresResponse>>.Success(sucursalesColaboradores);
        }
    }
}