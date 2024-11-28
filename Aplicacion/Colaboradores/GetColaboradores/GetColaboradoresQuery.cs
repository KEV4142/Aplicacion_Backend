using Aplicacion.Colaboradores.GetColaborador;
using Aplicacion.Core;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistencia;

namespace Aplicacion.Colaboradores.GetColaboradores;
public class GetColaboradoresQuery
{
    public record GetColaboradoresQueryRequest : IRequest<Result<List<ColaboradorResponse>>>
    {
    }

    internal class GetColaboradoresQueryHandler
        : IRequestHandler<GetColaboradoresQueryRequest, Result<List<ColaboradorResponse>>>
    {
        private readonly BackendContext _context;
        private readonly IMapper _mapper;

        public GetColaboradoresQueryHandler(BackendContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result<List<ColaboradorResponse>>> Handle(
            GetColaboradoresQueryRequest request, 
            CancellationToken cancellationToken
        )
        {
            var colaboradores = await _context.Colaboradores!
                .ProjectTo<ColaboradorResponse>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return Result<List<ColaboradorResponse>>.Success(colaboradores);
        }
    }
}
