using System.Linq;
using System.Linq.Expressions;
using Aplicacion.Core;
using Aplicacion.Sucursales.GetSucursal;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Modelo.entidades;
using Persistencia;

namespace Aplicacion.Sucursales.GetSucursalesPagin;
public class GetSucursalesPaginQuery
{
    public record GetSucursalesPaginQueryRequest : IRequest<Result<PagedList<SucursalResponse>>>
    {
        public GetSucursalesPaginRequest? SucursalesPaginRequest { get; set; }
    }
    internal class GetSucursalesPaginQueryHandler
    : IRequestHandler<GetSucursalesPaginQueryRequest, Result<PagedList<SucursalResponse>>>
    {
        private readonly BackendContext _context;
        private readonly IMapper _mapper;

        public GetSucursalesPaginQueryHandler(BackendContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result<PagedList<SucursalResponse>>> Handle(
            GetSucursalesPaginQueryRequest request,
            CancellationToken cancellationToken
        )
        {

            IQueryable<Sucursal> queryable = _context.Sucursales!;

            var predicate = ExpressionBuilder.New<Sucursal>();
            if (!string.IsNullOrEmpty(request.SucursalesPaginRequest!.Descripcion))
            {
                predicate = predicate
                .And(y => y.Descripcion!.ToUpper()
                .Contains(request.SucursalesPaginRequest.Descripcion.ToUpper()));
            }
            if (!string.IsNullOrEmpty(request.SucursalesPaginRequest!.Direccion))
            {
                predicate = predicate
                .And(y => y.Direccion!.ToUpper()
                .Contains(request.SucursalesPaginRequest.Direccion.ToUpper()));
            }
            if (!string.IsNullOrEmpty(request.SucursalesPaginRequest!.Estado))
            {
                predicate = predicate
                .And(y => y.Estado!.ToUpper()
                .Contains(request.SucursalesPaginRequest.Estado.ToUpper()));
            }

            if (!string.IsNullOrEmpty(request.SucursalesPaginRequest!.OrderBy))
            {
                Expression<Func<Sucursal, object>>? orderBySelector =
                                request.SucursalesPaginRequest.OrderBy!.ToUpper() switch
                                {
                                    "descripcion" => sucursal => sucursal.Descripcion!,
                                    "direccion" => sucursal => sucursal.Direccion!,
                                    "estado" => sucursal => sucursal.Estado!,
                                    _ => sucursal => sucursal.Descripcion!
                                };

                bool orderBy = request.SucursalesPaginRequest.OrderAsc.HasValue
                            ? request.SucursalesPaginRequest.OrderAsc.Value
                            : true;

                queryable = orderBy
                            ? queryable.OrderBy(orderBySelector)
                            : queryable.OrderByDescending(orderBySelector);
            }

            queryable = queryable.Where(predicate);

            var sucursalesQuery = queryable
            .ProjectTo<SucursalResponse>(_mapper.ConfigurationProvider)
            .AsQueryable();

            var pagination = await PagedList<SucursalResponse>.CreateAsync(
                sucursalesQuery,
                request.SucursalesPaginRequest.PageNumber,
                request.SucursalesPaginRequest.PageSize
            );

            return Result<PagedList<SucursalResponse>>.Success(pagination);

        }
    }
}
