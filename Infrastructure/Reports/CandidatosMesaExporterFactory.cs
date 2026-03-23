using Application.Reportes.Interfaces;

namespace Infrastructure.Reports;

public class CandidatosMesaExporterFactory : ICandidatosMesaExporterFactory
{
    private readonly CandidatosMesaPdfExporter _pdfExporter;
    private readonly CandidatosMesaXlsExporter _xlsExporter;

    public CandidatosMesaExporterFactory(
        CandidatosMesaPdfExporter pdfExporter,
        CandidatosMesaXlsExporter xlsExporter)
    {
        _pdfExporter = pdfExporter;
        _xlsExporter = xlsExporter;
    }

    public ICandidatosMesaExporter GetExporter(string formato) => formato.ToLowerInvariant() switch
    {
        "pdf"            => _pdfExporter,
        "xls" or "xlsx"  => _xlsExporter,
        _ => throw new ArgumentException($"Formato '{formato}' no soportado. Use 'pdf', 'xls' o 'json'.")
    };
}
