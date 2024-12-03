using System.Net;
using Aplicacion.Colaboradores.GetColaborador;
using Aplicacion.Colaboradores.GetColaboradoresPagin;
using Aplicacion.Core;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Modelo.entidades;
using static Aplicacion.Colaboradores.GetColaborador.GetColaborador;
using static Aplicacion.Colaboradores.GetColaboradores.GetColaboradoresQuery;
using static Aplicacion.Colaboradores.GetColaboradoresActivos.GetColaboradoresActivos;
using static Aplicacion.Colaboradores.GetColaboradoresPagin.GetColaboradoresPaginQuery;

namespace WebApi.Controllers;

[ApiController]
[Authorize]
[Route("api/colaboradores")]
public class ColaboradoresController : ControllerBase
{
    private readonly ISender _sender;
    public ColaboradoresController(ISender sender)
    {
        _sender = sender;
    }

    [Authorize(PolicyMaster.COLABORADOR_READ)]
    [HttpGet("{id}")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<ActionResult<ColaboradorResponse>> ColaboradorGet(
        int id,
        CancellationToken cancellationToken
    )
    {
        var query = new GetColaboradorQueryRequest { ColaboradorID = id };
        var resultado = await _sender.Send(query, cancellationToken);
        return resultado.IsSuccess ? Ok(resultado.Value) : BadRequest();
    }

    [Authorize(PolicyMaster.COLABORADOR_READ)]
    [HttpGet("listado")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult<ColaboradorResponse>> ColaboradoresGet(
        CancellationToken cancellationToken
    )
    {
        var query = new GetColaboradoresQueryRequest();
        var resultado = await _sender.Send(query, cancellationToken);
        return resultado.IsSuccess ? Ok(resultado.Value) : BadRequest();
    }

    [Authorize(PolicyMaster.COLABORADOR_READ)]
    [HttpGet("activos")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult<ColaboradorResponse>> ColaboradoresActivosGet(
        CancellationToken cancellationToken
    )
    {
        var query = new GetColaboradoresActivosQueryRequest();
        var resultado = await _sender.Send(query, cancellationToken);
        return resultado.IsSuccess ? Ok(resultado.Value) : BadRequest();
    }

    [Authorize(PolicyMaster.COLABORADOR_READ)]
    [HttpGet("paginacion")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<ActionResult<PagedList<ColaboradorResponse>>> PaginationColaboradores(
        [FromQuery] GetColaboradoresPaginRequest request,
        CancellationToken cancellationToken
    )
    {

        var query = new GetColaboradoresPaginQueryRequest { ColaboradoresPaginRequest = request };
        var resultado = await _sender.Send(query, cancellationToken);

        return resultado.IsSuccess ? Ok(resultado.Value) : NotFound();
    }
}
