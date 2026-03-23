namespace Application.Reportes.Interfaces;

public interface IDiaDExporterFactory
{
    IDiaDExporter GetExporter(string formato);
}
