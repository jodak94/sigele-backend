using Application.Reportes.DTOs;

namespace Application.Reportes.Interfaces;

public interface IReportExporter
{
    string ContentType { get; }
    string FileExtension { get; }
    byte[] Export(ReporteElectoresResult reporte);
}
