using Application.Reportes.DTOs;
using Application.Reportes.Interfaces;
using ClosedXML.Excel;

namespace Infrastructure.Reports;

public class ResumenOperadoresXlsExporter : IResumenOperadoresExporter
{
    public string ContentType   => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
    public string FileExtension => "xlsx";

    public byte[] Export(ResumenOperadoresData reporte)
    {
        using var workbook = new XLWorkbook();
        var ws = workbook.Worksheets.Add("Resumen");

        // Título
        var titulo = reporte.Coordinador is not null
            ? $"Resumen de Operadores — {reporte.Coordinador.Nombre}"
            : "Resumen de Operadores";

        ws.Cell(1, 1).Value = titulo;
        ws.Range(1, 1, 1, 5).Merge();
        ws.Cell(1, 1).Style.Font.Bold = true;
        ws.Cell(1, 1).Style.Font.FontSize = 14;

        ws.Cell(2, 1).Value = $"Generado el {DateTime.Now:dd/MM/yyyy HH:mm}";
        ws.Range(2, 1, 2, 5).Merge();
        ws.Cell(2, 1).Style.Font.FontColor = XLColor.Gray;
        ws.Cell(2, 1).Style.Font.FontSize = 9;

        // Encabezados
        var headers = new[]
        {
            "Nombre del Operador", "Teléfono",
            "Electores Captados", "Miembros de Mesa Disp.", "Req. Transporte"
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
        foreach (var item in reporte.Operadores)
        {
            ws.Cell(row, 1).Value = item.NombreOperador;
            ws.Cell(row, 2).Value = item.Telefono;
            ws.Cell(row, 3).Value = item.ElectoresCaptados;
            ws.Cell(row, 4).Value = item.MiembrosMesaDisp;
            ws.Cell(row, 5).Value = item.ReqTransporte;

            if (row % 2 == 0)
                ws.Range(row, 1, row, 5).Style.Fill.BackgroundColor = XLColor.FromHtml("#f9fafb");

            row++;
        }

        // Fila de totales
        var totalesRange = ws.Range(row, 1, row, 5);
        totalesRange.Style.Fill.BackgroundColor = XLColor.FromHtml("#f3f4f6");
        totalesRange.Style.Font.Bold = true;
        totalesRange.Style.Border.TopBorder = XLBorderStyleValues.Medium;
        totalesRange.Style.Border.TopBorderColor = XLColor.FromHtml("#d1d5db");

        ws.Cell(row, 1).Value = "TOTALES";
        ws.Cell(row, 2).Value = "";
        ws.Cell(row, 3).Value = reporte.Totales.ElectoresCaptados;
        ws.Cell(row, 4).Value = reporte.Totales.MiembrosMesaDisp;
        ws.Cell(row, 5).Value = reporte.Totales.ReqTransporte;

        // Anchos de columna
        ws.Column(1).Width = 35;
        ws.Column(2).Width = 16;
        ws.Column(3).Width = 20;
        ws.Column(4).Width = 24;
        ws.Column(5).Width = 18;

        ws.PageSetup.PageOrientation = XLPageOrientation.Landscape;

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return stream.ToArray();
    }
}
