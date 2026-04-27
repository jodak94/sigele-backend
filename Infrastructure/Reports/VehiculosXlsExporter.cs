using Application.Reportes.DTOs;
using Application.Reportes.Interfaces;
using ClosedXML.Excel;

namespace Infrastructure.Reports;

public class VehiculosXlsExporter : IVehiculosExporter
{
    public string ContentType   => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
    public string FileExtension => "xlsx";

    public byte[] Export(VehiculosReporteData reporte)
    {
        using var workbook = new XLWorkbook();
        var ws = workbook.Worksheets.Add("Vehículos");

        // Título
        ws.Cell(1, 1).Value = "Reporte de Vehículos";
        ws.Range(1, 1, 1, 6).Merge();
        ws.Cell(1, 1).Style.Font.Bold = true;
        ws.Cell(1, 1).Style.Font.FontSize = 14;

        ws.Cell(2, 1).Value = $"Generado el {DateTime.Now:dd/MM/yyyy HH:mm} — Total: {reporte.Total} vehículo(s)";
        ws.Range(2, 1, 2, 6).Merge();
        ws.Cell(2, 1).Style.Font.FontColor = XLColor.Gray;
        ws.Cell(2, 1).Style.Font.FontSize = 9;

        // Encabezados
        var headers = new[]
        {
            "Nombre del Dueño", "Teléfono", "Capacidad",
            "Monto Alquiler", "Operador", "Observación"
        };

        for (int i = 0; i < headers.Length; i++)
        {
            var cell = ws.Cell(4, i + 1);
            cell.Value = headers[i];
            cell.Style.Font.Bold = true;
            cell.Style.Fill.BackgroundColor = XLColor.FromHtml("#f3f4f6");
            cell.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            cell.Style.Border.BottomBorderColor = XLColor.FromHtml("#d1d5db");
            cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
        }

        // Datos
        int row = 5;
        foreach (var item in reporte.Vehiculos)
        {
            ws.Cell(row, 1).Value = item.NombreDueno;
            ws.Cell(row, 2).Value = item.TelefonoDueno;
            ws.Cell(row, 3).Value = item.Capacidad;
            ws.Cell(row, 4).Value = item.MontoAlquiler;
            ws.Cell(row, 5).Value = item.OperadorNombre ?? "";
            ws.Cell(row, 6).Value = item.Observacion ?? "";

            ws.Cell(row, 4).Style.NumberFormat.Format = "#,##0";

            if (row % 2 == 0)
                ws.Range(row, 1, row, 6).Style.Fill.BackgroundColor = XLColor.FromHtml("#f9fafb");

            row++;
        }

        // Anchos de columna
        ws.Column(1).Width = 30;
        ws.Column(2).Width = 16;
        ws.Column(3).Width = 12;
        ws.Column(4).Width = 16;
        ws.Column(5).Width = 28;
        ws.Column(6).Width = 35;

        ws.PageSetup.PageOrientation = XLPageOrientation.Landscape;

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return stream.ToArray();
    }
}
