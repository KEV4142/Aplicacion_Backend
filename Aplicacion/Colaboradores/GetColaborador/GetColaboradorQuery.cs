using Aplicacion.Core;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistencia;

namespace Aplicacion.Colaboradores.GetColaborador;

public class GetColaborador
{
    public record GetColaboradorQueryRequest
        : IRequest<Result<ColaboradorResponse>>
    {
        public int ColaboradorID { get; set; }
    }
    internal class GetColaboradorQueryHandler
    : IRequestHandler<GetColaboradorQueryRequest, Result<ColaboradorResponse>>
    {
        private readonly BackendContext _context;
        private readonly IMapper _mapper;

        public GetColaboradorQueryHandler(BackendContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result<ColaboradorResponse>> Handle(
            GetColaboradorQueryRequest request, 
            CancellationToken cancellationToken
        )
        {
            var colaborador = await _context.Colaboradores!.Where(x => x.ColaboradorID == request.ColaboradorID)
            .ProjectTo<ColaboradorResponse>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();

            return Result<ColaboradorResponse>.Success(colaborador!);
        }
    }
}
public record ColaboradorResponse(
    int ColaboradorID,
    string Nombre,
    string Direccion,
    string Coordenada,
    string Estado
);