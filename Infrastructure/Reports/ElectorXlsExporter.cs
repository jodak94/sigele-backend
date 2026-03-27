using Application.Reportes.DTOs;
using Application.Reportes.Interfaces;
using ClosedXML.Excel;

namespace Infrastructure.Reports;

public class ElectorXlsExporter : IReportExporter
{
    public string ContentType   => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
    public string FileExtension => "xlsx";

    public byte[] Export(ReporteElectoresResult reporte)
    {
        using var workbook = new XLWorkbook();
        var ws = workbook.Worksheets.Add("Electores");

        var headers = new[] { "Nro Documento", "Nombre", "Apellido", "Local de Votación", "Mesa", "Orden", "Firma / Asistencia" };

        // Título
        ws.Cell(1, 1).Value = $"Lista de Electores — {reporte.OperadorNombre}";
        ws.Range(1, 1, 1, headers.Length).Merge();
        ws.Cell(1, 1).Style.Font.Bold = true;
        ws.Cell(1, 1).Style.Font.FontSize = 14;

        ws.Cell(2, 1).Value = $"Generado el {DateTime.Now:dd/MM/yyyy HH:mm}";
        ws.Range(2, 1, 2, headers.Length).Merge();
        ws.Cell(2, 1).Style.Font.FontColor = XLColor.Gray;
        ws.Cell(2, 1).Style.Font.FontSize = 9;

        // Encabezados
        for (int i = 0; i < headers.Length; i++)
        {
            var cell = ws.Cell(4, i + 1);
            cell.Value = headers[i];
            cell.Style.Font.Bold = true;
            cell.Style.Fill.BackgroundColor = XLColor.FromHtml("#f3f4f6");
            cell.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            cell.Style.Border.BottomBorderColor = XLColor.FromHtml("#d1d5db");
        }

        // Datos
        int row = 5;
        foreach (var e in reporte.Electores)
        {
            ws.Cell(row, 1).Value = e.NroDocumento;
            ws.Cell(row, 2).Value = e.Nombre;
            ws.Cell(row, 3).Value = e.Apellido;
            ws.Cell(row, 4).Value = e.LocalVotacion ?? "";
            if (e.Mesa.HasValue)  ws.Cell(row, 5).Value = e.Mesa.Value;
            else                  ws.Cell(row, 5).Value = string.Empty;
            if (e.Orden.HasValue) ws.Cell(row, 6).Value = e.Orden.Value;
            else                  ws.Cell(row, 6).Value = string.Empty;

            if (row % 2 == 0)
                ws.Range(row, 1, row, headers.Length).Style.Fill.BackgroundColor = XLColor.FromHtml("#f9fafb");

            row++;
        }

        ws.Column(7).Width = 25; // Firma / Asistencia — ancho fijo para escribir a mano
        ws.Columns(1, 6).AdjustToContents();

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return stream.ToArray();
    }
}
