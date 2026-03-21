namespace Application.Reportes.Interfaces;

public interface IReportExporterFactory
{
    IReportExporter GetExporter(string formato);
}
