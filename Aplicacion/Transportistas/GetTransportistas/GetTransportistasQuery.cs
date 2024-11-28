using Aplicacion.Core;
using Aplicacion.Transportistas.GetTransportista;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistencia;
namespace Aplicacion.Transportistas.GetTransportistas;
public class GetTransportistas{
    public record GetTransportistasQueryRequest : IRequest<Result<List<TransportistaResponse>>>
    {
    }
    internal class GetTransportistasQueryHandler
        : IRequestHandler<GetTransportistasQueryRequest, Result<List<TransportistaResponse>>>
    {
        private readonly BackendContext _context;
        private readonly IMapper _mapper;

        public GetTransportistasQueryHandler(BackendContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result<List<TransportistaResponse>>> Handle(
            GetTransportistasQueryRequest request, 
            CancellationToken cancellationToken
        )
        {
            var transportistas = await _context.Transportistas!
                .ProjectTo<TransportistaResponse>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return Result<List<TransportistaResponse>>.Success(transportistas);
        }
    }
}
