namespace Application.Reportes.Interfaces;

public interface IListaAsistenciaExporterFactory
{
    IListaAsistenciaExporter GetExporter(string formato);
}
