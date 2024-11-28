using Aplicacion.Core;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistencia;

namespace Aplicacion.Sucursales.GetSucursal;
public class GetSucursal
{
    public record GetSucursalQueryRequest
        : IRequest<Result<SucursalResponse>>
    {
        public int SucursalID { get; set; }
    }

    internal class GetSucursalQueryHandler
    : IRequestHandler<GetSucursalQueryRequest, Result<SucursalResponse>>
    {
        private readonly BackendContext _context;
        private readonly IMapper _mapper;

        public GetSucursalQueryHandler(BackendContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result<SucursalResponse>> Handle(
            GetSucursalQueryRequest request, 
            CancellationToken cancellationToken
        )
        {
            var sucursal = await _context.Sucursales!.Where(x => x.SucursalID == request.SucursalID)
            // .Include(x=>x.Viajes)
            .ProjectTo<SucursalResponse>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();

            return Result<SucursalResponse>.Success(sucursal!);
        }


    }
}
public record SucursalResponse(
    int SucursalID,
    string Descripcion,
    string Direccion,
    string Coordenada,
    string Estado
);