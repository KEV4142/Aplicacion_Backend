using Aplicacion.Reporte;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Modelo.entidades;



namespace WebApi.Controllers;

[ApiController]
[Route("api/reporte")]
public class ExportarDocumentoController : ControllerBase
{
    private readonly ISender _sender;
    public ExportarDocumentoController(ISender sender)
    {
        _sender = sender;
    }

    [Authorize(PolicyMaster.VIAJE_READ)]
    [Authorize(PolicyMaster.VIAJEDETALLE_READ)]
    [HttpPost] 
    public async Task<ActionResult> GetTask(
        [FromBody] ExportPDF.Consulta request,
        CancellationToken cancellationToken)
    {
        var command = new ExportPDF.Consulta(request.TransportistaID,request.FechaInicio,request.FechaFinal);
        var resultado = await _sender.Send(command, cancellationToken);
        return File(resultado, "application/pdf", "reporte_transportistas.pdf");
    }
}



