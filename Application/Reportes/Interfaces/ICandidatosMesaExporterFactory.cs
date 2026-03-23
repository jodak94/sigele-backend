namespace Application.Reportes.Interfaces;

public interface ICandidatosMesaExporterFactory
{
    ICandidatosMesaExporter GetExporter(string formato);
}
