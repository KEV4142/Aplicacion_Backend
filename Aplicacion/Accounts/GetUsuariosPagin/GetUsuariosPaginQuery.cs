using System.Linq.Expressions;
using Aplicacion.Core;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Persistencia.Models;
using Microsoft.AspNetCore.Identity;

namespace Aplicacion.Accounts.GetUsuariosPagin;
public class GetUsuariosPaginQuery
{
    public record GetUsuariosPaginQueryRequest : IRequest<Result<PagedList<UsuarioResponse>>>
    {
        public GetUsuariosPaginRequest? UsuariosPaginRequest { get; set; }
    }

    internal class GetUsuariosPaginQueryHandler
    : IRequestHandler<GetUsuariosPaginQueryRequest, Result<PagedList<UsuarioResponse>>>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;

        public GetUsuariosPaginQueryHandler(UserManager<AppUser> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<Result<PagedList<UsuarioResponse>>> Handle(
            GetUsuariosPaginQueryRequest request,
            CancellationToken cancellationToken
        )
        {

            IQueryable<AppUser> queryable = _userManager.Users!;

            var predicate = ExpressionBuilder.New<AppUser>();
            if (!string.IsNullOrEmpty(request.UsuariosPaginRequest!.Email))
            {
                predicate = predicate
                .And(y => y.Email!.ToUpper()
                .Contains(request.UsuariosPaginRequest.Email.ToUpper()));
            }
            if (!string.IsNullOrEmpty(request.UsuariosPaginRequest!.Username))
            {
                predicate = predicate
                .And(y => y.UserName!.ToUpper()
                .Contains(request.UsuariosPaginRequest.Username.ToUpper()));
            }

            if (!string.IsNullOrEmpty(request.UsuariosPaginRequest!.OrderBy))
            {
                Expression<Func<AppUser, object>>? orderBySelector =
                                request.UsuariosPaginRequest.OrderBy!.ToUpper() switch
                                {
                                    "email" => usuario => usuario.Email!,
                                    "username" => usuario => usuario.UserName!,
                                    _ => usuario => usuario.Email!
                                };

                bool orderBy = request.UsuariosPaginRequest.OrderAsc.HasValue
                            ? request.UsuariosPaginRequest.OrderAsc.Value
                            : true;

                queryable = orderBy
                            ? queryable.OrderBy(orderBySelector)
                            : queryable.OrderByDescending(orderBySelector);
            }

            queryable = queryable.Where(predicate);

            var usuariosQuery = queryable.Select(user => new UsuarioResponse(
                                user.Id,
                                user.NombreCompleto ?? "Nombre Completo",
                                user.Email ?? "Email",
                                user.UserName ?? "Usuario"
                                   ));


            var pagination = await PagedList<UsuarioResponse>.CreateAsync(
                usuariosQuery,
                request.UsuariosPaginRequest.PageNumber,
                request.UsuariosPaginRequest.PageSize
            );

            return Result<PagedList<UsuarioResponse>>.Success(pagination);

        }
    }
}
public record UsuarioResponse(
    string UsuarioID,
    string NombreCompleto,
    string Email,
    string Username
);