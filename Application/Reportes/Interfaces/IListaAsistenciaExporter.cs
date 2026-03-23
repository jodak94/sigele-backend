using Application.Reportes.DTOs;

namespace Application.Reportes.Interfaces;

public interface IListaAsistenciaExporter
{
    string ContentType { get; }
    string FileExtension { get; }
    byte[] Export(ListaAsistenciaResult reporte);
}
