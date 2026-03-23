using Application.Reportes.Interfaces;

namespace Infrastructure.Reports;

public class ResumenOperadoresExporterFactory : IResumenOperadoresExporterFactory
{
    private readonly ResumenOperadoresPdfExporter _pdfExporter;
    private readonly ResumenOperadoresXlsExporter _xlsExporter;

    public ResumenOperadoresExporterFactory(
        ResumenOperadoresPdfExporter pdfExporter,
        ResumenOperadoresXlsExporter xlsExporter)
    {
        _pdfExporter = pdfExporter;
        _xlsExporter = xlsExporter;
    }

    public IResumenOperadoresExporter GetExporter(string formato) => formato.ToLowerInvariant() switch
    {
        "pdf"            => _pdfExporter,
        "xls" or "xlsx"  => _xlsExporter,
        _ => throw new ArgumentException($"Formato '{formato}' no soportado. Use 'pdf', 'xls' o 'json'.")
    };
}
