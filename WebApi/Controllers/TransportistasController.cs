using Aplicacion.Transportistas.TransportistaCreate;
using static Aplicacion.Transportistas.TransportistaCreate.TransportistaCreateCommand;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Aplicacion.Core;
using System.Net;
using Aplicacion.Transportistas.GetTransportista;
using static Aplicacion.Transportistas.GetTransportista.GetTransportista;
using Aplicacion.Transportistas.TransportistaUpdate;
using static Aplicacion.Transportistas.TransportistaUpdate.TransportistaUpdateCommand;
using static Aplicacion.Transportistas.GetTransportistas.GetTransportistas;
using static Aplicacion.Transportistas.GetTransportistasActivos.GetTransportistasActivos;
using Microsoft.AspNetCore.Authorization;
using Modelo.entidades;
using Aplicacion.Transportistas.GetTransportistasPagin;
using static Aplicacion.Transportistas.GetTransportistasPagin.GetTransportistasPaginQuery;

namespace WebApi.Controllers;

[ApiController]
[Route("api/transportistas")]
public class TransportistasController : ControllerBase
{
    private readonly ISender _sender;
    public TransportistasController(ISender sender)
    {
        _sender = sender;
    }

    // [FromBody] o [FromForm]
    [Authorize(PolicyMaster.TRANSPORTISTA_WRITE)]
    [HttpPost("registro")]
    public async Task<ActionResult<Result<int>>> TransportistaCreate(
        [FromBody] TransportistaCreateRequest request,
        CancellationToken cancellationToken)
    {
        var command = new TransportistaCreateCommandRequest(request);
        var resultado = await _sender.Send(command, cancellationToken);
        return resultado;
    }

    [Authorize(PolicyMaster.TRANSPORTISTA_READ)]
    [HttpGet("listado")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult<TransportistaResponse>> TransportistasGet(
        CancellationToken cancellationToken
    )
    {
        var query = new GetTransportistasQueryRequest();
        var resultado = await _sender.Send(query, cancellationToken);
        return resultado.IsSuccess ? Ok(resultado.Value) : BadRequest();
    }

    [Authorize(PolicyMaster.TRANSPORTISTA_READ)]
    [HttpGet("activos")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult<TransportistaResponse>> GetTransportistasActivos(
        CancellationToken cancellationToken
    )
    {
        var query = new GetTransportistasActivosQueryRequest();
        var resultado = await _sender.Send(query, cancellationToken);
        return resultado.IsSuccess ? Ok(resultado.Value) : BadRequest();
    }

    [Authorize(PolicyMaster.TRANSPORTISTA_READ)]
    [HttpGet("{id}")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<ActionResult<TransportistaResponse>> TransportistaGet(
        int id,
        CancellationToken cancellationToken
    )
    {
        var query = new GetTransportistaQueryRequest { TransportistaID = id };
        var resultado = await _sender.Send(query, cancellationToken);
        return resultado.IsSuccess ? Ok(resultado.Value) : BadRequest();
    }

    [Authorize(PolicyMaster.TRANSPORTISTA_READ)]
    [HttpGet("paginacion")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<ActionResult<PagedList<TransportistaResponse>>> PaginationTransportistas(
        [FromQuery] GetTransportistasPaginRequest request,
        CancellationToken cancellationToken
    )
    {

        var query = new GetTransportistasPaginQueryRequest { TransportistasPaginRequest = request };
        var resultado = await _sender.Send(query, cancellationToken);

        return resultado.IsSuccess ? Ok(resultado.Value) : NotFound();
    }

    [Authorize(PolicyMaster.TRANSPORTISTA_UPDATE)]
    [HttpPut("{id}")]
    public async Task<ActionResult<Result<int>>> TransportistaUpdate(
        [FromBody] TransportistaUpdateRequest request,
        int id,
        CancellationToken cancellationToken
    )
    {
        var command = new TransportistaUpdateCommandRequest(request, id);
        var resultado = await _sender.Send(command, cancellationToken);
        return resultado.IsSuccess ? Ok(resultado.Value) : BadRequest();
    }
}
