using Application.Reportes.DTOs;
using Application.Reportes.Interfaces;
using ClosedXML.Excel;

namespace Infrastructure.Reports;

public class ElectorXlsExporter : IReportExporter
{
    public string ContentType => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
    public string FileExtension => "xlsx";

    public byte[] Export(ReporteElectoresResult reporte)
    {
        using var workbook = new XLWorkbook();
        var ws = workbook.Worksheets.Add("Electores");

        // Título
        ws.Cell(1, 1).Value = $"Lista de Electores — {reporte.OperadorNombre}";
        ws.Range(1, 1, 1, 6).Merge();
        ws.Cell(1, 1).Style.Font.Bold = true;
        ws.Cell(1, 1).Style.Font.FontSize = 14;

        ws.Cell(2, 1).Value = $"Generado el {DateTime.Now:dd/MM/yyyy HH:mm}";
        ws.Range(2, 1, 2, 6).Merge();
        ws.Cell(2, 1).Style.Font.FontColor = XLColor.Gray;
        ws.Cell(2, 1).Style.Font.FontSize = 9;

        // Encabezados
        var headers = new[] { "Nombre", "Nro Documento", "Teléfono", "Miembro de mesa", "Requiere Transporte", "Dirección" };
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
        foreach (var e in reporte.Electores)
        {
            ws.Cell(row, 1).Value = $"{e.Apellido} {e.Nombre}".Trim();
            ws.Cell(row, 2).Value = e.NroDocumento;
            ws.Cell(row, 3).Value = e.NroTelefono;
            ws.Cell(row, 4).Value = e.MiembroMesa ? "Si" : "No";
            ws.Cell(row, 5).Value = e.RequiereTransporte ? "Si" : "No";
            ws.Cell(row, 6).Value = e.Direccion ?? "";

            if (row % 2 == 0)
            {
                ws.Range(row, 1, row, 6).Style.Fill.BackgroundColor = XLColor.FromHtml("#f9fafb");
            }

            row++;
        }

        ws.Columns().AdjustToContents();
        ws.Column(1).Width = Math.Max(ws.Column(1).Width, 30);
        ws.Column(6).Width = Math.Max(ws.Column(6).Width, 30);

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return stream.ToArray();
    }
}
