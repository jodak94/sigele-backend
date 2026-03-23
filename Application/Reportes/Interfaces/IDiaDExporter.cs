using Application.Reportes.DTOs;

namespace Application.Reportes.Interfaces;

public interface IDiaDExporter
{
    string ContentType   { get; }
    string FileExtension { get; }
    byte[] Export(DiaDResult reporte);
}
