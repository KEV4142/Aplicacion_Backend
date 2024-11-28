using Aplicacion.Core;
using Aplicacion.Transportistas.GetTransportista;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistencia;
namespace Aplicacion.Transportistas.GetTransportistasActivos;
public class GetTransportistasActivos
{
    public record GetTransportistasActivosQueryRequest : IRequest<Result<List<TransportistaResponse>>>
    {
    }

    internal class GetTransportistasQueryHandler
        : IRequestHandler<GetTransportistasActivosQueryRequest, Result<List<TransportistaResponse>>>
    {
        private readonly BackendContext _context;
        private readonly IMapper _mapper;

        public GetTransportistasQueryHandler(BackendContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result<List<TransportistaResponse>>> Handle(
            GetTransportistasActivosQueryRequest request, 
            CancellationToken cancellationToken
        )
        {
            var transportistas = await _context.Transportistas!
                .Where(t => t.Estado!=null && t.Estado.ToUpper() =="A")
                .ProjectTo<TransportistaResponse>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return Result<List<TransportistaResponse>>.Success(transportistas);
        }
    }
}
