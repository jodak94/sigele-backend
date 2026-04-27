using Application.Reportes.Interfaces;
using Application.Reportes.UseCases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Application.Common.Attributes;
using Application.Common.Constants;

namespace Api.Controllers;

[ApiController]
[Route("api/reportes")]
[Authorize]
public class ReportesController : ControllerBase
{
    private readonly GetReporteElectoresPorOperador    _getReporte;
    private readonly IReportExporterFactory             _exporterFactory;
    private readonly GetResumenOperadores               _getResumenOperadores;
    private readonly IResumenOperadoresExporterFactory  _resumenOperadoresExporterFactory;
    private readonly GetReporteDiaD                     _getReporteDiaD;
    private readonly IDiaDExporterFactory               _diaDExporterFactory;
    private readonly GetReporteCandidatosMesa           _getReporteCandidatosMesa;
    private readonly ICandidatosMesaExporterFactory     _candidatosMesaExporterFactory;
    private readonly GetReporteVehiculos                _getReporteVehiculos;
    private readonly IVehiculosExporterFactory          _vehiculosExporterFactory;

    public ReportesController(
        GetReporteElectoresPorOperador getReporte,
        IReportExporterFactory exporterFactory,
        GetResumenOperadores getResumenOperadores,
        IResumenOperadoresExporterFactory resumenOperadoresExporterFactory,
        GetReporteDiaD getReporteDiaD,
        IDiaDExporterFactory diaDExporterFactory,
        GetReporteCandidatosMesa getReporteCandidatosMesa,
        ICandidatosMesaExporterFactory candidatosMesaExporterFactory,
        GetReporteVehiculos getReporteVehiculos,
        IVehiculosExporterFactory vehiculosExporterFactory)
    {
        _getReporte                       = getReporte;
        _exporterFactory                  = exporterFactory;
        _getResumenOperadores             = getResumenOperadores;
        _resumenOperadoresExporterFactory = resumenOperadoresExporterFactory;
        _getReporteDiaD                   = getReporteDiaD;
        _diaDExporterFactory              = diaDExporterFactory;
        _getReporteCandidatosMesa         = getReporteCandidatosMesa;
        _candidatosMesaExporterFactory    = candidatosMesaExporterFactory;
        _getReporteVehiculos              = getReporteVehiculos;
        _vehiculosExporterFactory         = vehiculosExporterFactory;
    }

    [HttpGet("electores-por-operador")]
    public async Task<IActionResult> GetElectoresPorOperador(
        [FromQuery] string formato,
        [FromQuery] int?   operadorId,
        CancellationToken  cancellationToken)
    {
        try
        {
            var reporte = await _getReporte.ExecuteAsync(operadorId, cancellationToken);

            if (formato.ToLowerInvariant() == "json")
                return Ok(reporte);

            var exporter = _exporterFactory.GetExporter(formato);
            var bytes    = exporter.Export(reporte);
            return File(bytes, exporter.ContentType, $"electores.{exporter.FileExtension}");
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("resumen-operadores")]
    public async Task<IActionResult> GetResumenOperadores(
        [FromQuery] string formato,
        [FromQuery] short? codigoSeccional,
        [FromQuery] int?   coordinadorId,
        CancellationToken  cancellationToken)
    {
        try
        {
            var reporte = await _getResumenOperadores.ExecuteAsync(codigoSeccional, coordinadorId, cancellationToken);

            if (formato.ToLowerInvariant() == "json")
                return Ok(reporte);

            var exporter = _resumenOperadoresExporterFactory.GetExporter(formato);
            var bytes    = exporter.Export(reporte);
            return File(bytes, exporter.ContentType, $"resumen-operadores.{exporter.FileExtension}");
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("dia-d")]
    public async Task<IActionResult> GetDiaD(
        [FromQuery] string formato,
        CancellationToken  cancellationToken)
    {
        try
        {
            var reporte = await _getReporteDiaD.ExecuteAsync(cancellationToken);

            if (formato.ToLowerInvariant() == "json")
                return Ok(reporte);

            var exporter = _diaDExporterFactory.GetExporter(formato);
            var bytes    = exporter.Export(reporte);
            return File(bytes, exporter.ContentType, $"dia-d.{exporter.FileExtension}");
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("candidatos-mesa")]
    public async Task<IActionResult> GetCandidatosMesa(
        [FromQuery] string formato,
        CancellationToken  cancellationToken)
    {
        try
        {
            var reporte = await _getReporteCandidatosMesa.ExecuteAsync(cancellationToken);

            if (formato.ToLowerInvariant() == "json")
                return Ok(reporte);

            var exporter = _candidatosMesaExporterFactory.GetExporter(formato);
            var bytes    = exporter.Export(reporte);
            return File(bytes, exporter.ContentType, $"candidatos-mesa.{exporter.FileExtension}");
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("vehiculos")]
    [RequiresPermission(Permissions.Vehiculos.Read)]
    public async Task<IActionResult> GetVehiculos(
        [FromQuery] string formato,
        CancellationToken  cancellationToken)
    {
        try
        {
            var reporte = await _getReporteVehiculos.ExecuteAsync(cancellationToken);

            if (formato.ToLowerInvariant() == "json")
                return Ok(reporte);

            var exporter = _vehiculosExporterFactory.GetExporter(formato);
            var bytes    = exporter.Export(reporte);
            return File(bytes, exporter.ContentType, $"vehiculos.{exporter.FileExtension}");
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
