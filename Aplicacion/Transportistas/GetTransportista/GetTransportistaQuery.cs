using Aplicacion.Core;
using Aplicacion.Viajes.GetViaje;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistencia;

namespace Aplicacion.Transportistas.GetTransportista;
public class GetTransportista
{
    public record GetTransportistaQueryRequest
        : IRequest<Result<TransportistaResponse>>
    {
        public int TransportistaID { get; set; }
    }

    internal class GetTransportistaQueryHandler
    : IRequestHandler<GetTransportistaQueryRequest, Result<TransportistaResponse>>
    {
        private readonly BackendContext _context;
        private readonly IMapper _mapper;

        public GetTransportistaQueryHandler(BackendContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result<TransportistaResponse>> Handle(
            GetTransportistaQueryRequest request, 
            CancellationToken cancellationToken
        )
        {
            var transportista = await _context.Transportistas!.Where(x => x.TransportistaID == request.TransportistaID)
            // .Include(x=>x.Viajes)
            .ProjectTo<TransportistaResponse>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();

            return Result<TransportistaResponse>.Success(transportista!);
        }
    }
}
public record TransportistaResponse(
    int TransportistaID,
    string Descripcion,
    decimal Tarifa,
    string? Estado
    // List<ViajeResponse>? Viajes
);