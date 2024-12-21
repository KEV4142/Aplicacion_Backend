using Aplicacion.Core;
using Aplicacion.Interfaces;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Modelo.entidades;
using Persistencia.Models;

namespace Aplicacion.Accounts.UsuarioUpdate;
public class UsuarioUpdateCommand{
    public record UsuarioUpdateCommandRequest(UsuarioUpdateRequest usuarioUpdateRequest, string UsuarioID) : IRequest<Result<Profile>>;

    internal class UsuarioUpdateCommandHandler : IRequestHandler<UsuarioUpdateCommandRequest, Result<Profile>>
    {
        
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;

        public UsuarioUpdateCommandHandler(
            UserManager<AppUser> userManager, 
            ITokenService tokenService
        )
        {
            _userManager = userManager;
            _tokenService = tokenService;
        }

        public async Task<Result<Profile>> Handle(UsuarioUpdateCommandRequest request, CancellationToken cancellationToken)
        {
            var tipo="";
            if (!request.usuarioUpdateRequest.Tipo!.Equals("Administrador")&&!request.usuarioUpdateRequest.Tipo!.Equals("Operador"))
            {
                return Result<Profile>.Failure("El campo Tipo de Usuario no contiene la opcion correcta.");
            }
            
           if (request.usuarioUpdateRequest.Tipo!.Equals("Administrador"))
            {
                tipo=CustomRoles.ADMIN;
            }
            else if (request.usuarioUpdateRequest.Tipo!.Equals("Operador"))
            {
                tipo=CustomRoles.CLIENT;
            }

            var usuario = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == request.UsuarioID);
            
            
            if (usuario is null)
            {
                return Result<Profile>.Failure("El Usuario no existe");
            }

            if(await  _userManager.Users
            .AnyAsync(x=> x.Email == request.usuarioUpdateRequest.Email && x.Id != usuario.Id))
            {
                return Result<Profile>.Failure("El email ya fue registrado por otro usuario");
            }

            if(await _userManager.Users
            .AnyAsync(x=>x.UserName == request.usuarioUpdateRequest.Username && x.Id != usuario.Id))
            {
                return Result<Profile>.Failure("El username ya fue registrado");
            }


            usuario.NombreCompleto = request.usuarioUpdateRequest.NombreCompleto;
            usuario.Email = request.usuarioUpdateRequest.Email;
            usuario.UserName  = request.usuarioUpdateRequest.Username;

            var resultado =  await _userManager.UpdateAsync(usuario);

            if(resultado.Succeeded)
            {
                var obtenerRoles = await _userManager.GetRolesAsync(usuario);
                if (!obtenerRoles.Equals(tipo)){
                    var retiroResultado = await _userManager.RemoveFromRolesAsync(usuario, obtenerRoles);
                    if (!retiroResultado.Succeeded)
                    {
                        return Result<Profile>.Failure("Error en la Actualizacion del Tipo.");
                    }
                    await _userManager.AddToRoleAsync(usuario, tipo);
                }
                
                var profile = new Profile
                {
                    Email = usuario.Email,
                    NombreCompleto = usuario.NombreCompleto,
                    Token = await _tokenService.CreateToken(usuario),
                    Username = usuario.UserName,
                    Tipo=request.usuarioUpdateRequest.Tipo
                };

                return Result<Profile>.Success(profile);
            }

            return Result<Profile>.Failure("Errores en la Actualizacion del usuario.");
        }
    }
    public class UpdateCommandRequestValidator : AbstractValidator<UsuarioUpdateCommandRequest>
    {
        public UpdateCommandRequestValidator()
        {
            RuleFor(x => x.usuarioUpdateRequest).SetValidator(new UsuarioUpdateValidator());
            RuleFor(x => x.UsuarioID).NotNull();
        }
    }
}