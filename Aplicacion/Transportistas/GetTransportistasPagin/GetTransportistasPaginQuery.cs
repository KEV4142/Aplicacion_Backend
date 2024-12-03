using System.Linq.Expressions;
using Aplicacion.Core;
using Aplicacion.Transportistas.GetTransportista;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Modelo.entidades;
using Persistencia;

namespace Aplicacion.Transportistas.GetTransportistasPagin;
public class GetTransportistasPaginQuery
{
    public record GetTransportistasPaginQueryRequest : IRequest<Result<PagedList<TransportistaResponse>>>
    {
        public GetTransportistasPaginRequest? TransportistasPaginRequest { get; set; }
    }

    internal class GetTransportistasPaginQueryHandler
    : IRequestHandler<GetTransportistasPaginQueryRequest, Result<PagedList<TransportistaResponse>>>
    {
        private readonly BackendContext _context;
        private readonly IMapper _mapper;

        public GetTransportistasPaginQueryHandler(BackendContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result<PagedList<TransportistaResponse>>> Handle(
            GetTransportistasPaginQueryRequest request,
            CancellationToken cancellationToken
        )
        {

            IQueryable<Transportista> queryable = _context.Transportistas!;

            var predicate = ExpressionBuilder.New<Transportista>();
            if (!string.IsNullOrEmpty(request.TransportistasPaginRequest!.Descripcion))
            {
                predicate = predicate
                .And(y => y.Descripcion!.ToLower()
                .Contains(request.TransportistasPaginRequest.Descripcion.ToLower()));
            }

            if (!string.IsNullOrEmpty(request.TransportistasPaginRequest!.OrderBy))
            {
                Expression<Func<Transportista, object>>? orderBySelector =
                                request.TransportistasPaginRequest.OrderBy!.ToLower() switch
                                {
                                    "descripcion" => transportista => transportista.Descripcion!,
                                    _ => transportista => transportista.Descripcion!
                                };

                bool orderBy = request.TransportistasPaginRequest.OrderAsc.HasValue
                            ? request.TransportistasPaginRequest.OrderAsc.Value
                            : true;

                queryable = orderBy
                            ? queryable.OrderBy(orderBySelector)
                            : queryable.OrderByDescending(orderBySelector);
            }

            queryable = queryable.Where(predicate);

            var transportistasQuery = queryable
            .ProjectTo<TransportistaResponse>(_mapper.ConfigurationProvider)
            .AsQueryable();

            var pagination = await PagedList<TransportistaResponse>.CreateAsync(
                transportistasQuery,
                request.TransportistasPaginRequest.PageNumber,
                request.TransportistasPaginRequest.PageSize
            );

            return Result<PagedList<TransportistaResponse>>.Success(pagination);

        }
    }
}
