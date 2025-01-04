using System.Net;
using Aplicacion.Core;
using Aplicacion.Sucursales.GetSucursal;
using Aplicacion.Sucursales.GetSucursalesPagin;
using Aplicacion.Sucursales.SucursalesCreate;
using Aplicacion.Sucursales.SucursalesUpdate;
using Aplicacion.Sucursales.SucursalesUpdateEstado;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Modelo.entidades;
using static Aplicacion.Sucursales.GetSucursal.GetSucursal;
using static Aplicacion.Sucursales.GetSucursales.GetSucursales;
using static Aplicacion.Sucursales.GetSucursalesActivas.GetSucursalesActivas;
using static Aplicacion.Sucursales.GetSucursalesPagin.GetSucursalesPaginQuery;
using static Aplicacion.Sucursales.SucursalesCreate.SucursalesCreateCommand;
using static Aplicacion.Sucursales.SucursalesUpdate.SucursalesUpdateCommand;
using static Aplicacion.Sucursales.SucursalesUpdateEstado.SucursalesUpdateEstadoCommand;

namespace WebApi.Controllers;

[ApiController]
[Route("api/sucursales")]
public class SucursalesController:ControllerBase
{
    private readonly ISender _sender;
    public SucursalesController(ISender sender)
    {
        _sender = sender;
    }
    
    [Authorize(PolicyMaster.SUCURSAL_CREATE)]
    [HttpPost("registro")]
    public async Task<ActionResult<Result<int>>> SucursalCreate(
        [FromBody] SucursalesCreateRequest request,
        CancellationToken cancellationToken)
    {
        var command = new SucursalesCreateCommandRequest(request);
        var resultado = await _sender.Send(command, cancellationToken);
        return resultado.IsSuccess ? Ok(resultado.Value) : BadRequest(resultado);;
    }

    [Authorize(PolicyMaster.SUCURSAL_UPDATE)]
    [HttpPut("{id}")]
    public async Task<ActionResult<Result<int>>> SucursalUpdate(
        [FromBody] SucursalesUpdateRequest request,
        int id,
        CancellationToken cancellationToken
    )
    {
        var command = new SucursalesUpdateCommandRequest(request, id);
        var resultado = await _sender.Send(command, cancellationToken);
        return resultado.IsSuccess ? Ok(resultado.Value) : BadRequest(resultado);
    }

    [Authorize(PolicyMaster.SUCURSAL_UPDATE)]
    [HttpPut("estado/{id}")]
    public async Task<ActionResult<Result<int>>> SucursalUpdateEstado(
        [FromBody] SucursalesUpdateEstadoRequest request,
        int id,
        CancellationToken cancellationToken
    )
    {
        var command = new SucursalesUpdateEstadoCommandRequest(request, id);
        var resultado = await _sender.Send(command, cancellationToken);
        return resultado.IsSuccess ? Ok(resultado) : BadRequest(resultado);
    }

    [Authorize(PolicyMaster.SUCURSAL_READ)]
    [HttpGet("{id}")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<ActionResult<SucursalResponse>> SucursalGet(
        int id,
        CancellationToken cancellationToken
    )
    {
        var query = new GetSucursalQueryRequest { SucursalID = id };
        var resultado = await _sender.Send(query, cancellationToken);
        return resultado.IsSuccess ? Ok(resultado.Value) : BadRequest();
    }

    [Authorize(PolicyMaster.SUCURSAL_READ)]
    [HttpGet("listado")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult<SucursalResponse>> SucursalesGet(
        CancellationToken cancellationToken
    )
    {
        var query = new GetSucursalesQueryRequest();
        var resultado = await _sender.Send(query, cancellationToken);
        return resultado.IsSuccess ? Ok(resultado.Value) : BadRequest();
    }

    [Authorize(PolicyMaster.SUCURSAL_READ)]
    [HttpGet("activos")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult<SucursalResponse>> GetSucursalesActivos(
        CancellationToken cancellationToken
    )
    {
        var query = new GetSucursalesActivasQueryRequest();
        var resultado = await _sender.Send(query, cancellationToken);
        return resultado.IsSuccess ? Ok(resultado.Value) : BadRequest();
    }

    [Authorize(PolicyMaster.SUCURSAL_READ)]
    [HttpGet("paginacion")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<ActionResult<PagedList<SucursalResponse>>> PaginationSucursales(
        [FromQuery] GetSucursalesPaginRequest request,
        CancellationToken cancellationToken
    )
    {

        var query = new GetSucursalesPaginQueryRequest { SucursalesPaginRequest = request };
        var resultado = await _sender.Send(query, cancellationToken);

        return resultado.IsSuccess ? Ok(resultado.Value) : NotFound();
    }
}
