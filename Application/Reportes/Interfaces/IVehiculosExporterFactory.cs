namespace Application.Reportes.Interfaces;

public interface IVehiculosExporterFactory
{
    IVehiculosExporter GetExporter(string formato);
}
