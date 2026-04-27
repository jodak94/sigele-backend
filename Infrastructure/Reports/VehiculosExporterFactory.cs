using Application.Reportes.Interfaces;

namespace Infrastructure.Reports;

public class VehiculosExporterFactory : IVehiculosExporterFactory
{
    private readonly VehiculosPdfExporter _pdfExporter;
    private readonly VehiculosXlsExporter _xlsExporter;

    public VehiculosExporterFactory(VehiculosPdfExporter pdfExporter, VehiculosXlsExporter xlsExporter)
    {
        _pdfExporter = pdfExporter;
        _xlsExporter = xlsExporter;
    }

    public IVehiculosExporter GetExporter(string formato) => formato.ToLowerInvariant() switch
    {
        "pdf"           => _pdfExporter,
        "xls" or "xlsx" => _xlsExporter,
        _ => throw new ArgumentException($"Formato '{formato}' no soportado. Use 'pdf', 'xls' o 'json'.")
    };
}
