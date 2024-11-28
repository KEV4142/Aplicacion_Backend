using Aplicacion.Colaboradores.GetColaborador;
using Aplicacion.Core;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistencia;

namespace Aplicacion.Colaboradores.GetColaboradoresActivos;
public class GetColaboradoresActivos
{
    public record GetColaboradoresActivosQueryRequest : IRequest<Result<List<ColaboradorResponse>>>
    {
    }

    internal class GetColaboradoresQueryHandler
        : IRequestHandler<GetColaboradoresActivosQueryRequest, Result<List<ColaboradorResponse>>>
    {
        private readonly BackendContext _context;
        private readonly IMapper _mapper;

        public GetColaboradoresQueryHandler(BackendContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result<List<ColaboradorResponse>>> Handle(
            GetColaboradoresActivosQueryRequest request, 
            CancellationToken cancellationToken
        )
        {
            var colaboradores = await _context.Colaboradores!
                .Where(t => t.Estado!=null && t.Estado.ToUpper() =="A")
                .ProjectTo<ColaboradorResponse>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return Result<List<ColaboradorResponse>>.Success(colaboradores);
        }
    }
}
