using Aplicacion.Core;
using Aplicacion.Sucursales.GetSucursal;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistencia;

namespace Aplicacion.Sucursales.GetSucursales;
public class GetSucursales
{
    public record GetSucursalesQueryRequest : IRequest<Result<List<SucursalResponse>>>
    {
    }
    internal class GetSucursalesQueryHandler
        : IRequestHandler<GetSucursalesQueryRequest, Result<List<SucursalResponse>>>
    {
        private readonly BackendContext _context;
        private readonly IMapper _mapper;

        public GetSucursalesQueryHandler(BackendContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result<List<SucursalResponse>>> Handle(
            GetSucursalesQueryRequest request, 
            CancellationToken cancellationToken
        )
        {
            var sucursales = await _context.Sucursales!
                .ProjectTo<SucursalResponse>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return Result<List<SucursalResponse>>.Success(sucursales);
        }
    }
}
