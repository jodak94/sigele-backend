using Application.Reportes.DTOs;
using Application.Reportes.Interfaces;
using ClosedXML.Excel;

namespace Infrastructure.Reports;

public class ListaAsistenciaXlsExporter : IListaAsistenciaExporter
{
    public string ContentType   => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
    public string FileExtension => "xlsx";

    public byte[] Export(ListaAsistenciaResult reporte)
    {
        using var workbook = new XLWorkbook();
        var ws = workbook.Worksheets.Add("Asistencia");

        // Título
        ws.Cell(1, 1).Value = $"Lista de Asistencia — {reporte.OperadorNombre}";
        ws.Range(1, 1, 1, 7).Merge();
        ws.Cell(1, 1).Style.Font.Bold = true;
        ws.Cell(1, 1).Style.Font.FontSize = 14;

        ws.Cell(2, 1).Value = $"Generado el {DateTime.Now:dd/MM/yyyy HH:mm}";
        ws.Range(2, 1, 2, 7).Merge();
        ws.Cell(2, 1).Style.Font.FontColor = XLColor.Gray;
        ws.Cell(2, 1).Style.Font.FontSize = 9;

        // Encabezados
        var headers = new[]
        {
            "N° Cédula", "Nombre y Apellido", "Teléfono",
            "Local de Votación", "Mesa", "Orden", "Firma / Asistencia"
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
        foreach (var item in reporte.Items)
        {
            ws.Cell(row, 1).Value = item.NroCedula;
            ws.Cell(row, 2).Value = item.NombreApellido;
            ws.Cell(row, 3).Value = item.Telefono;
            ws.Cell(row, 4).Value = item.LocalVotacion ?? "";
            if (item.Mesa.HasValue)  ws.Cell(row, 5).Value = item.Mesa.Value;
            else                     ws.Cell(row, 5).Value = string.Empty;
            if (item.Orden.HasValue) ws.Cell(row, 6).Value = item.Orden.Value;
            else                     ws.Cell(row, 6).Value = string.Empty;
            // columna 7 (Firma/Asistencia) se deja vacía

            if (row % 2 == 0)
                ws.Range(row, 1, row, 7).Style.Fill.BackgroundColor = XLColor.FromHtml("#f9fafb");

            row++;
        }

        // Anchos de columna
        ws.Column(1).Width = 14;  // N° Cédula
        ws.Column(2).Width = 35;  // Nombre y Apellido
        ws.Column(3).Width = 16;  // Teléfono
        ws.Column(4).Width = 35;  // Local de Votación
        ws.Column(5).Width = 8;   // Mesa
        ws.Column(6).Width = 8;   // Orden
        ws.Column(7).Width = 30;  // Firma/Asistencia

        ws.PageSetup.PageOrientation = XLPageOrientation.Landscape;

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return stream.ToArray();
    }
}
