using System.Linq.Expressions;
using Aplicacion.Core;
using Aplicacion.SucursalesColaboradores.GetSucursalColaboradoresActivos;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Modelo.entidades;
using Persistencia;

namespace Aplicacion.SucursalesColaboradores.GetSucursalesColaboradoresPagin;
public class GetSucursalesColaboradoresPaginQuery
{
    public record GetSucursalesColaboradoresPaginQueryRequest : IRequest<Result<PagedList<SucursalColaboradoresActivosResponse>>>
    {
        public GetSucursalesColaboradoresPaginRequest? SucursalesColaboradoresPaginRequest { get; set; }
    }
    internal class GetSucursalesPaginQueryHandler
    : IRequestHandler<GetSucursalesColaboradoresPaginQueryRequest, Result<PagedList<SucursalColaboradoresActivosResponse>>>
    {
        private readonly BackendContext _context;
        private readonly IMapper _mapper;

        public GetSucursalesPaginQueryHandler(BackendContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result<PagedList<SucursalColaboradoresActivosResponse>>> Handle(
            GetSucursalesColaboradoresPaginQueryRequest request,
            CancellationToken cancellationToken
        )
        {
            IQueryable<SucursalColaborador> queryable = _context.SucursalesColaboradores!
                                                            .Include(sc => sc.Colaborador)
                                                            .Include(sc => sc.Sucursal);

            var predicate = ExpressionBuilder.New<SucursalColaborador>();
            if (!string.IsNullOrEmpty(request.SucursalesColaboradoresPaginRequest!.ColaboradorID.ToString()))
            {
                predicate = predicate
                .And(y => y.ColaboradorID!
                .Equals(request.SucursalesColaboradoresPaginRequest.ColaboradorID));
            }
            if (!string.IsNullOrEmpty(request.SucursalesColaboradoresPaginRequest!.SucursalID.ToString()))
            {
                predicate = predicate
                .And(y => y.SucursalID!
                .Equals(request.SucursalesColaboradoresPaginRequest.SucursalID));
            }
            if (!string.IsNullOrEmpty(request.SucursalesColaboradoresPaginRequest!.Estado))
            {
                predicate = predicate
                .And(y => y.Estado!.ToUpper()
                .Contains(request.SucursalesColaboradoresPaginRequest.Estado.ToUpper()));
            }

            queryable = queryable.Where(predicate);

            if (!string.IsNullOrEmpty(request.SucursalesColaboradoresPaginRequest!.OrderBy))
            {
                Expression<Func<SucursalColaborador, object>>? orderBySelector =
                                request.SucursalesColaboradoresPaginRequest.OrderBy!.ToUpper() switch
                                {
                                    "sucursalid" => sucursalColaborador => sucursalColaborador.SucursalID!,
                                    "colaboradorid" => sucursalColaborador => sucursalColaborador.ColaboradorID!,
                                    "estado" => sucursalColaborador => sucursalColaborador.Estado!,
                                    _ => sucursalColaborador => sucursalColaborador.SucursalID!
                                };

                bool orderBy = request.SucursalesColaboradoresPaginRequest.OrderAsc.HasValue
                            ? request.SucursalesColaboradoresPaginRequest.OrderAsc.Value
                            : true;

                queryable = orderBy
                            ? queryable.OrderBy(orderBySelector)
                            : queryable.OrderByDescending(orderBySelector);
            }


            var queryResultado = queryable.Select(sc => new SucursalColaboradoresActivosResponse(
                                                    sc.SucursalID,
                                                    sc.Sucursal.Descripcion,
                                                    sc.ColaboradorID,
                                                    Funciones.ToProperCase(sc.Colaborador.Nombre),
                                                    sc.Distancia,
                                                    sc.Estado
                                                ));
            var pagination = await PagedList<SucursalColaboradoresActivosResponse>.CreateAsync(
                queryResultado,
                request.SucursalesColaboradoresPaginRequest.PageNumber,
                request.SucursalesColaboradoresPaginRequest.PageSize
            );

            return Result<PagedList<SucursalColaboradoresActivosResponse>>.Success(pagination);

        }
    }
}
