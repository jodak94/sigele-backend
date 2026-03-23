using Application.Reportes.DTOs;

namespace Application.Reportes.Interfaces;

public interface IResumenOperadoresExporter
{
    string ContentType   { get; }
    string FileExtension { get; }
    byte[] Export(ResumenOperadoresData reporte);
}
