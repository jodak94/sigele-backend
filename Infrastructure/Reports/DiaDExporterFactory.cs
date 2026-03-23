using Application.Reportes.Interfaces;

namespace Infrastructure.Reports;

public class DiaDExporterFactory : IDiaDExporterFactory
{
    private readonly DiaDPdfExporter _pdfExporter;
    private readonly DiaDXlsExporter _xlsExporter;

    public DiaDExporterFactory(DiaDPdfExporter pdfExporter, DiaDXlsExporter xlsExporter)
    {
        _pdfExporter = pdfExporter;
        _xlsExporter = xlsExporter;
    }

    public IDiaDExporter GetExporter(string formato) => formato.ToLowerInvariant() switch
    {
        "pdf"            => _pdfExporter,
        "xls" or "xlsx"  => _xlsExporter,
        _ => throw new ArgumentException($"Formato '{formato}' no soportado. Use 'pdf', 'xls' o 'json'.")
    };
}
