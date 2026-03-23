using Application.Reportes.Interfaces;

namespace Infrastructure.Reports;

public class ListaAsistenciaExporterFactory : IListaAsistenciaExporterFactory
{
    private readonly ListaAsistenciaPdfExporter _pdfExporter;
    private readonly ListaAsistenciaXlsExporter _xlsExporter;

    public ListaAsistenciaExporterFactory(
        ListaAsistenciaPdfExporter pdfExporter,
        ListaAsistenciaXlsExporter xlsExporter)
    {
        _pdfExporter = pdfExporter;
        _xlsExporter = xlsExporter;
    }

    public IListaAsistenciaExporter GetExporter(string formato) => formato.ToLowerInvariant() switch
    {
        "pdf"          => _pdfExporter,
        "xls" or "xlsx" => _xlsExporter,
        _ => throw new ArgumentException($"Formato '{formato}' no soportado. Use 'pdf', 'xls' o 'json'.")
    };
}
