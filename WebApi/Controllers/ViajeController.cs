using Aplicacion.Core;
using Aplicacion.Viajes.ViajeCreate;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Modelo.entidades;
using static Aplicacion.Viajes.ViajeCreate.ViajeCreateCommand;

namespace WebApi.Controllers;

[ApiController]
[Route("api/viaje")]
public class ViajeController : ControllerBase
{
    private readonly ISender _sender;

    public ViajeController(ISender sender)
    {
        _sender = sender;
    }

    [Authorize(Policy = PolicyMaster.VIAJE_CREATE)]
    [Authorize(Policy = PolicyMaster.VIAJEDETALLE_CREATE)]
    [HttpPost("registro")]
    public async Task<ActionResult<Result<int>>> ViajeCreate(
        [FromBody] ViajeCreateRequest request,
        CancellationToken cancellationToken)
    {
        var command = new ViajeCreateCommandRequest(request);
        var resultado = await _sender.Send(command, cancellationToken);
        return resultado.IsSuccess ? Ok(resultado) : BadRequest(resultado);
    }
}
