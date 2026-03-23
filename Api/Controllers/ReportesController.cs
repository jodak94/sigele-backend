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
    private readonly GetReporteElectoresPorOperador   _getReporte;
    private readonly IReportExporterFactory            _exporterFactory;
    private readonly GetListaAsistencia                _getListaAsistencia;
    private readonly IListaAsistenciaExporterFactory   _listaAsistenciaExporterFactory;
    private readonly GetResumenOperadores              _getResumenOperadores;
    private readonly IResumenOperadoresExporterFactory _resumenOperadoresExporterFactory;
    private readonly GetReporteDiaD                      _getReporteDiaD;
    private readonly IDiaDExporterFactory                _diaDExporterFactory;
    private readonly GetReporteCandidatosMesa            _getReporteCandidatosMesa;
    private readonly ICandidatosMesaExporterFactory      _candidatosMesaExporterFactory;

    public ReportesController(
        GetReporteElectoresPorOperador getReporte,
        IReportExporterFactory exporterFactory,
        GetListaAsistencia getListaAsistencia,
        IListaAsistenciaExporterFactory listaAsistenciaExporterFactory,
        GetResumenOperadores getResumenOperadores,
        IResumenOperadoresExporterFactory resumenOperadoresExporterFactory,
        GetReporteDiaD getReporteDiaD,
        IDiaDExporterFactory diaDExporterFactory,
        GetReporteCandidatosMesa getReporteCandidatosMesa,
        ICandidatosMesaExporterFactory candidatosMesaExporterFactory)
    {
        _getReporte                       = getReporte;
        _exporterFactory                  = exporterFactory;
        _getListaAsistencia               = getListaAsistencia;
        _listaAsistenciaExporterFactory   = listaAsistenciaExporterFactory;
        _getResumenOperadores             = getResumenOperadores;
        _resumenOperadoresExporterFactory = resumenOperadoresExporterFactory;
        _getReporteDiaD                   = getReporteDiaD;
        _diaDExporterFactory              = diaDExporterFactory;
        _getReporteCandidatosMesa         = getReporteCandidatosMesa;
        _candidatosMesaExporterFactory    = candidatosMesaExporterFactory;
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

    [HttpGet("lista-asistencia")]
    public async Task<IActionResult> GetListaAsistencia(
        [FromQuery] int operadorId,
        [FromQuery] string formato,
        CancellationToken cancellationToken)
    {
        try
        {
            var reporte = await _getListaAsistencia.ExecuteAsync(operadorId, cancellationToken);

            if (formato.ToLowerInvariant() == "json")
                return Ok(reporte.Items);

            var exporter = _listaAsistenciaExporterFactory.GetExporter(formato);
            var bytes    = exporter.Export(reporte);
            return File(bytes, exporter.ContentType, $"lista-asistencia.{exporter.FileExtension}");
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
        [FromQuery] string  formato,
        [FromQuery] short?  codigoSeccional,
        CancellationToken   cancellationToken)
    {
        try
        {
            var reporte = await _getResumenOperadores.ExecuteAsync(codigoSeccional, cancellationToken);

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
}
