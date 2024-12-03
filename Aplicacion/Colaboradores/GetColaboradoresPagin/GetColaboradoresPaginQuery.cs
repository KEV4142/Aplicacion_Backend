using System.Linq.Expressions;
using Aplicacion.Colaboradores.GetColaborador;
using Aplicacion.Core;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Modelo.entidades;
using Persistencia;

namespace Aplicacion.Colaboradores.GetColaboradoresPagin;
public class GetColaboradoresPaginQuery
{
    public record GetColaboradoresPaginQueryRequest : IRequest<Result<PagedList<ColaboradorResponse>>>
    {
        public GetColaboradoresPaginRequest? ColaboradoresPaginRequest { get; set; }
    }
    internal class GetColaboradoresPaginQueryHandler
    : IRequestHandler<GetColaboradoresPaginQueryRequest, Result<PagedList<ColaboradorResponse>>>
    {
        private readonly BackendContext _context;
        private readonly IMapper _mapper;

        public GetColaboradoresPaginQueryHandler(BackendContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result<PagedList<ColaboradorResponse>>> Handle(
            GetColaboradoresPaginQueryRequest request,
            CancellationToken cancellationToken
        )
        {

            IQueryable<Colaborador> queryable = _context.Colaboradores!;

            var predicate = ExpressionBuilder.New<Colaborador>();
            if (!string.IsNullOrEmpty(request.ColaboradoresPaginRequest!.Nombre))
            {
                predicate = predicate
                .And(y => y.Nombre!.ToUpper()
                .Contains(request.ColaboradoresPaginRequest.Nombre.ToUpper()));
            }
            if (!string.IsNullOrEmpty(request.ColaboradoresPaginRequest!.Direccion))
            {
                predicate = predicate
                .And(y => y.Direccion!.ToUpper()
                .Contains(request.ColaboradoresPaginRequest.Direccion.ToUpper()));
            }
            if (!string.IsNullOrEmpty(request.ColaboradoresPaginRequest!.Estado))
            {
                predicate = predicate
                .And(y => y.Estado!.ToUpper()
                .Contains(request.ColaboradoresPaginRequest.Estado.ToUpper()));
            }

            if (!string.IsNullOrEmpty(request.ColaboradoresPaginRequest!.OrderBy))
            {
                Expression<Func<Colaborador, object>>? orderBySelector =
                                request.ColaboradoresPaginRequest.OrderBy!.ToUpper() switch
                                {
                                    "nombre" => colaborador => colaborador.Nombre!,
                                    "direccion" => colaborador => colaborador.Direccion!,
                                    "estado" => colaborador => colaborador.Estado!,
                                    _ => colaborador => colaborador.Nombre!
                                };

                bool orderBy = request.ColaboradoresPaginRequest.OrderAsc.HasValue
                            ? request.ColaboradoresPaginRequest.OrderAsc.Value
                            : true;

                queryable = orderBy
                            ? queryable.OrderBy(orderBySelector)
                            : queryable.OrderByDescending(orderBySelector);
            }

            queryable = queryable.Where(predicate);

            var colaboradoresQuery = queryable
            .ProjectTo<ColaboradorResponse>(_mapper.ConfigurationProvider)
            .AsQueryable();

            var pagination = await PagedList<ColaboradorResponse>.CreateAsync(
                colaboradoresQuery,
                request.ColaboradoresPaginRequest.PageNumber,
                request.ColaboradoresPaginRequest.PageSize
            );

            return Result<PagedList<ColaboradorResponse>>.Success(pagination);

        }
    }
}
