using System.Net;
using Aplicacion.Core;
using Aplicacion.Sucursales.GetSucursal;
using Aplicacion.Sucursales.GetSucursalesPagin;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Modelo.entidades;
using static Aplicacion.Sucursales.GetSucursal.GetSucursal;
using static Aplicacion.Sucursales.GetSucursales.GetSucursales;
using static Aplicacion.Sucursales.GetSucursalesActivas.GetSucursalesActivas;
using static Aplicacion.Sucursales.GetSucursalesPagin.GetSucursalesPaginQuery;

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
