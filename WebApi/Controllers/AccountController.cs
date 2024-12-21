
using System.Net;
using Aplicacion.Accounts;
using Aplicacion.Accounts.GetCurrentUser;
using Aplicacion.Accounts.GetUsuariosPagin;
using Aplicacion.Accounts.Login;
using Aplicacion.Accounts.UsuarioCreate;
using Aplicacion.Accounts.UsuarioUpdate;
using Aplicacion.Accounts.UsuarioUpdatePassword;
using Aplicacion.Core;
using Aplicacion.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Modelo.entidades;
using static Aplicacion.Accounts.GetCurrentUser.GetCurrentUserQuery;
using static Aplicacion.Accounts.GetUsuariosPagin.GetUsuariosPaginQuery;
using static Aplicacion.Accounts.Login.LoginCommand;
using static Aplicacion.Accounts.UsuarioCreate.UsuarioCreateCommand;
using static Aplicacion.Accounts.UsuarioUpdate.UsuarioUpdateCommand;
using static Aplicacion.Accounts.UsuarioUpdatePassword.UsuarioUpdatePasswordCommand;

namespace WebApi.Controllers;
[ApiController]
[Route("api/account")]
public class AccountController : ControllerBase
{
    private readonly ISender _sender;
    private readonly IUserAccessor _user;
    public AccountController(ISender sender, IUserAccessor user)
    {
        _sender = sender;
        _user = user;
    }

    [AllowAnonymous]
    [HttpPost("login")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<ActionResult<Profile>> Login(
        [FromBody] LoginRequest request,
        CancellationToken cancellationToken
    )
    {
        var command = new LoginCommandRequest(request);
        var resultado = await _sender.Send(command, cancellationToken);
        return resultado.IsSuccess ? Ok(resultado.Value) : Unauthorized();
    }

    [Authorize]
    [HttpGet("me")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<ActionResult<Profile>> Me(CancellationToken cancellationToken)
    {
        var email = _user.GetEmail();
        var request = new GetCurrentUserRequest {Email = email};
        var query = new GetCurrentUserQueryRequest(request);
        var resultado =  await _sender.Send(query, cancellationToken);
        return resultado.IsSuccess ? Ok(resultado.Value) : Unauthorized();
    }
    [Authorize(PolicyMaster.USUARIO_CREATE)]
    [HttpPost("agregar")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<ActionResult<Profile>> AgregarUsuario(
        [FromBody] UsuarioCreateRequest request,
        CancellationToken cancellationToken
    )
    {
        var command = new UsuarioCreateCommandRequest(request);
        var resultado = await _sender.Send(command, cancellationToken);
        return resultado.IsSuccess ? Ok(resultado.Value) : Unauthorized(resultado);
    }

    [Authorize(PolicyMaster.USUARIO_UPDATE)]
    [HttpPut("{id}")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<ActionResult<Profile>> ActualizarUsuario(
        [FromBody] UsuarioUpdateRequest request,
        string id,
        CancellationToken cancellationToken
    )
    {
        var command = new UsuarioUpdateCommandRequest(request,id);
        var resultado = await _sender.Send(command, cancellationToken);
        return resultado.IsSuccess ? Ok(resultado.Value) : Unauthorized(resultado);
    }

    [Authorize(PolicyMaster.USUARIO_UPDATE)]
    [HttpPut("password/{id}")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<ActionResult<Profile>> ActualizarPasswordUsuario(
        [FromBody] UsuarioUpdatePasswordRequest request,
        string id,
        CancellationToken cancellationToken
    )
    {
        var command = new UsuarioUpdatePasswordCommandRequest(request,id);
        var resultado = await _sender.Send(command, cancellationToken);
        return resultado.IsSuccess ? Ok(resultado.Value) : Unauthorized(resultado);
    }

    [Authorize(Roles = CustomRoles.ADMIN)]
    [HttpGet("paginacion")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<ActionResult<PagedList<UsuarioResponse>>> PaginationUsuarios(
        [FromQuery] GetUsuariosPaginRequest request,
        CancellationToken cancellationToken
    )
    {

        var query = new GetUsuariosPaginQueryRequest { UsuariosPaginRequest = request };
        var resultado = await _sender.Send(query, cancellationToken);

        return resultado.IsSuccess ? Ok(resultado.Value) : NotFound();
    }
}