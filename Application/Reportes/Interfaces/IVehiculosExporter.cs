using Application.Reportes.DTOs;

namespace Application.Reportes.Interfaces;

public interface IVehiculosExporter
{
    string ContentType   { get; }
    string FileExtension { get; }
    byte[] Export(VehiculosReporteData reporte);
}
