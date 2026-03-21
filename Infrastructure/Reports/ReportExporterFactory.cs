using Application.Reportes.Interfaces;

namespace Infrastructure.Reports;

public class ReportExporterFactory : IReportExporterFactory
{
    private readonly ElectorXlsExporter _xlsExporter;
    private readonly ElectorPdfExporter _pdfExporter;

    public ReportExporterFactory(ElectorXlsExporter xlsExporter, ElectorPdfExporter pdfExporter)
    {
        _xlsExporter = xlsExporter;
        _pdfExporter = pdfExporter;
    }

    public IReportExporter GetExporter(string formato) => formato.ToLowerInvariant() switch
    {
        "xls" or "xlsx" => _xlsExporter,
        "pdf"           => _pdfExporter,
        _               => throw new ArgumentException($"Formato de reporte no soportado: '{formato}'. Use 'xls' o 'pdf'.")
    };
}
