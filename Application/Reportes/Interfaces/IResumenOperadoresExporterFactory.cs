namespace Application.Reportes.Interfaces;

public interface IResumenOperadoresExporterFactory
{
    IResumenOperadoresExporter GetExporter(string formato);
}
