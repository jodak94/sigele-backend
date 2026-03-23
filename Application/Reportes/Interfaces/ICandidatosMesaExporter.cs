using Application.Reportes.DTOs;

namespace Application.Reportes.Interfaces;

public interface ICandidatosMesaExporter
{
    string ContentType   { get; }
    string FileExtension { get; }
    byte[] Export(CandidatosMesaResult reporte);
}
