using Aplicacion.Core;
using Aplicacion.Interfaces;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Modelo.entidades;
using Persistencia.Models;

namespace Aplicacion.Accounts.UsuarioCreate;
public class UsuarioCreateCommand
{
    public record UsuarioCreateCommandRequest(UsuarioCreateRequest usuarioCreateRequest) : IRequest<Result<Profile>>;
    
    internal class UsuarioCreateCommandHandler : IRequestHandler<UsuarioCreateCommandRequest, Result<Profile>>
    {
        
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;

        public UsuarioCreateCommandHandler(
            UserManager<AppUser> userManager, 
            ITokenService tokenService
        )
        {
            _userManager = userManager;
            _tokenService = tokenService;
        }

        public async Task<Result<Profile>> Handle(UsuarioCreateCommandRequest request, CancellationToken cancellationToken)
        {
            var tipo="";
           if (request.usuarioCreateRequest.Tipo!.Equals("Administrador"))
            {
                tipo=CustomRoles.ADMIN;
            }
            else if (request.usuarioCreateRequest.Tipo!.Equals("Operador"))
            {
                tipo=CustomRoles.CLIENT;
            }
            if (!request.usuarioCreateRequest.Tipo!.Equals("Administrador")&&!request.usuarioCreateRequest.Tipo!.Equals("Operador"))
            {
                return Result<Profile>.Failure("El campo Tipo de Usuario no contiene la opcion correcta.");
            }

            if(await  _userManager.Users
            .AnyAsync(x=> x.Email == request.usuarioCreateRequest.Email))
            {
                return Result<Profile>.Failure("El email ya fue registrado por otro usuario");
            }

            if(await _userManager.Users
            .AnyAsync(x=>x.UserName == request.usuarioCreateRequest.Username))
            {
                return Result<Profile>.Failure("El username ya fue registrado");
            }

             var user = new AppUser
             {
                NombreCompleto = request.usuarioCreateRequest.NombreCompleto,
                Id = Guid.NewGuid().ToString(),
                Email = request.usuarioCreateRequest.Email,
                UserName  = request.usuarioCreateRequest.Username
             };
           
            var resultado =  await _userManager
            .CreateAsync(user, request.usuarioCreateRequest.Password!);

            if(resultado.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, tipo);
                
                var profile = new Profile
                {
                    Email = user.Email,
                    NombreCompleto = user.NombreCompleto,
                    Token = await _tokenService.CreateToken(user),
                    Username = user.UserName,
                    Tipo=request.usuarioCreateRequest.Tipo
                };

                return Result<Profile>.Success(profile);
            }

            return Result<Profile>.Failure("Errores en el registro del nuevo usuario.");
        }
    }

    public class RegiterCommandRequestValidator : AbstractValidator<UsuarioCreateCommandRequest>
    {
        public RegiterCommandRequestValidator()
        {
            RuleFor(x => x.usuarioCreateRequest).SetValidator(new UsuarioCreateValidator());
        }
    }

}
