using Application.Reportes.DTOs;
using Application.Reportes.Interfaces;
using ClosedXML.Excel;

namespace Infrastructure.Reports;

public class DiaDXlsExporter : IDiaDExporter
{
    public string ContentType   => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
    public string FileExtension => "xlsx";

    public byte[] Export(DiaDResult reporte)
    {
        using var workbook = new XLWorkbook();
        var ws = workbook.Worksheets.Add("Día D");

        var headers = new[]
        {
            "Mesa", "Orden", "Cédula", "Elector",
            "Teléfono", "Dir. Recogida", "Transporte", "Operador Responsable"
        };

        int row = 1;

        foreach (var local in reporte.Locales)
        {
            // Encabezado del local
            ws.Cell(row, 1).Value = $"Local: {local.LocalVotacion}";
            var localRange = ws.Range(row, 1, row, headers.Length);
            localRange.Merge();
            localRange.Style.Font.Bold = true;
            localRange.Style.Font.FontSize = 11;
            localRange.Style.Fill.BackgroundColor = XLColor.FromHtml("#e5e7eb");
            localRange.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            localRange.Style.Border.BottomBorderColor = XLColor.FromHtml("#d1d5db");
            row++;

            // Encabezados de columna
            for (int i = 0; i < headers.Length; i++)
            {
                var cell = ws.Cell(row, i + 1);
                cell.Value = headers[i];
                cell.Style.Font.Bold = true;
                cell.Style.Fill.BackgroundColor = XLColor.FromHtml("#f3f4f6");
                cell.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                cell.Style.Border.BottomBorderColor = XLColor.FromHtml("#d1d5db");
            }
            row++;

            // Filas de datos
            foreach (var e in local.Electores)
            {
                if (e.Mesa.HasValue)  ws.Cell(row, 1).Value = e.Mesa.Value;
                else                  ws.Cell(row, 1).Value = string.Empty;
                if (e.Orden.HasValue) ws.Cell(row, 2).Value = e.Orden.Value;
                else                  ws.Cell(row, 2).Value = string.Empty;
                ws.Cell(row, 3).Value = e.NroCedula;
                ws.Cell(row, 4).Value = e.NombreApellido;
                ws.Cell(row, 5).Value = e.Telefono;
                ws.Cell(row, 6).Value = string.IsNullOrWhiteSpace(e.DireccionRecogida) ? "—" : e.DireccionRecogida;
                ws.Cell(row, 7).Value = e.RequiereTransporte ? "Sí" : "No";
                ws.Cell(row, 8).Value = e.OperadorResponsable;
                row++;
            }

            row++; // espacio entre locales
        }

        // Anchos de columna
        ws.Column(1).Width = 7;   // Mesa
        ws.Column(2).Width = 7;   // Orden
        ws.Column(3).Width = 13;  // Cédula
        ws.Column(4).Width = 30;  // Elector
        ws.Column(5).Width = 15;  // Teléfono
        ws.Column(6).Width = 30;  // Dir. Recogida
        ws.Column(7).Width = 12;  // Transporte
        ws.Column(8).Width = 25;  // Operador

        ws.PageSetup.PageOrientation = XLPageOrientation.Landscape;

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return stream.ToArray();
    }
}
