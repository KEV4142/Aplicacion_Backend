using System.Net;
using Aplicacion.Core;
using Aplicacion.SucursalesColaboradores.GetSucursalColaboradores;
using Aplicacion.SucursalesColaboradores.GetSucursalColaboradoresActivos;
using Aplicacion.SucursalesColaboradores.GetSucursalesColaboradoresPagin;
using Aplicacion.SucursalesColaboradores.SucursalesColaboradoresCreate;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Modelo.entidades;
using static Aplicacion.SucursalesColaboradores.GetSucursalColaboradores.GetSucursalColaboradores;
using static Aplicacion.SucursalesColaboradores.GetSucursalColaboradoresActivos.GetSucursalColaboradoresActivos;
using static Aplicacion.SucursalesColaboradores.GetSucursalesColaboradores.GetSucursalesColaboradores;
using static Aplicacion.SucursalesColaboradores.GetSucursalesColaboradoresActivas.GetSucursalesColaboradoresActivas;
using static Aplicacion.SucursalesColaboradores.GetSucursalesColaboradoresPagin.GetSucursalesColaboradoresPaginQuery;
using static Aplicacion.SucursalesColaboradores.SucursalesColaboradoresCreate.SucursalesColaboradoresCreateCommand;

namespace WebApi.Controllers;

[ApiController]
[Route("api/sucursalescolaboradores")]
public class SucursalesColaboradoresController:ControllerBase
{
    private readonly ISender _sender;

    public SucursalesColaboradoresController(ISender sender)
    {
        _sender = sender;
    }

    [Authorize(PolicyMaster.SUCURSALCOLABORADOR_CREATE)]
    [HttpPost("registro")]
    public async Task<ActionResult<Result<int>>> SucursalesColaboradoresCreate(
        [FromBody] SucursalesColaboradoresCreateRequest request,
        CancellationToken cancellationToken)
    {
        var command = new SucursalesColaboradoresCreateCommandRequest(request);
        var resultado = await _sender.Send(command, cancellationToken);
        return resultado.IsSuccess ? Ok(resultado) : BadRequest(resultado);
    }

    [Authorize(PolicyMaster.SUCURSALCOLABORADOR_READ)]
    [HttpGet("{id}")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<ActionResult<SucursalesColaboradoresResponse>> SucursalColaboradoresGet(
        int id,
        CancellationToken cancellationToken
    )
    {
        var query = new GetSucursalColaboradoresQueryRequest { SucursalID = id };
        var resultado = await _sender.Send(query, cancellationToken);
        return resultado.IsSuccess ? Ok(resultado.Value) : BadRequest();
    }

    [Authorize(PolicyMaster.SUCURSALCOLABORADOR_READ)]
    [HttpGet("listado")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult<SucursalesColaboradoresResponse>> SucursalesGet(
        CancellationToken cancellationToken
    )
    {
        var query = new GetSucursalesColaboradoresQueryRequest();
        var resultado = await _sender.Send(query, cancellationToken);
        return resultado.IsSuccess ? Ok(resultado.Value) : BadRequest();
    }

    [Authorize(PolicyMaster.SUCURSALCOLABORADOR_READ)]
    [HttpGet("activos")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult<SucursalesColaboradoresResponse>> GetSucursalesColaboradoresActivos(
        CancellationToken cancellationToken
    )
    {
        var query = new GetSucursalesColaboradoresActivasQueryRequest();
        var resultado = await _sender.Send(query, cancellationToken);
        return resultado.IsSuccess ? Ok(resultado.Value) : BadRequest();
    }
    [Authorize(PolicyMaster.SUCURSALCOLABORADOR_READ)]
    [HttpGet("activos/{id}")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<ActionResult<SucursalesColaboradoresResponse>> SucursalColaboradoresActivosGet(
        int id,
        CancellationToken cancellationToken
    )
    {
        var query = new GetSucursalColaboradoresActivosQueryRequest { SucursalID = id };
        var resultado = await _sender.Send(query, cancellationToken);
        return resultado.IsSuccess ? Ok(resultado.Value) : BadRequest();
    }
    [Authorize(PolicyMaster.SUCURSALCOLABORADOR_READ)]
    [HttpGet("paginacion")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<ActionResult<PagedList<SucursalColaboradoresActivosResponse>>> PaginationSucursalesColaboradores(
        [FromQuery] GetSucursalesColaboradoresPaginRequest request,
        CancellationToken cancellationToken
    )
    {

        var query = new GetSucursalesColaboradoresPaginQueryRequest { SucursalesColaboradoresPaginRequest = request };
        var resultado = await _sender.Send(query, cancellationToken);

        return resultado.IsSuccess ? Ok(resultado.Value) : NotFound();
    }
}
