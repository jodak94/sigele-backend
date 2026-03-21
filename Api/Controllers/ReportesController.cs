using Application.Reportes.Interfaces;
using Application.Reportes.UseCases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/reportes")]
[Authorize]
public class ReportesController : ControllerBase
{
    private readonly GetReporteElectoresPorOperador _getReporte;
    private readonly IReportExporterFactory _exporterFactory;

    public ReportesController(
        GetReporteElectoresPorOperador getReporte,
        IReportExporterFactory exporterFactory)
    {
        _getReporte = getReporte;
        _exporterFactory = exporterFactory;
    }

    [HttpGet("electores-por-operador")]
    public async Task<IActionResult> GetElectoresPorOperador(
        [FromQuery] string formato,
        CancellationToken cancellationToken)
    {
        try
        {
            var reporte = await _getReporte.ExecuteAsync(cancellationToken);
            var exporter = _exporterFactory.GetExporter(formato);
            var bytes = exporter.Export(reporte);
            return File(bytes, exporter.ContentType, $"electores.{exporter.FileExtension}");
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
