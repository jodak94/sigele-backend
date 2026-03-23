using Application.Reportes.DTOs;
using Application.Reportes.Interfaces;
using ClosedXML.Excel;

namespace Infrastructure.Reports;

public class CandidatosMesaXlsExporter : ICandidatosMesaExporter
{
    public string ContentType   => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
    public string FileExtension => "xlsx";

    public byte[] Export(CandidatosMesaResult reporte)
    {
        using var workbook = new XLWorkbook();
        var ws = workbook.Worksheets.Add("Candidatos a Mesa");

        var headers = new[]
        {
            "Nombre del Candidato a Mesa", "Cédula", "Teléfono",
            "Vota en Mesa N°", "Operador Responsable", "Mesa Asignada / OBS"
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
            foreach (var c in local.Candidatos)
            {
                ws.Cell(row, 1).Value = c.NombreApellido;
                ws.Cell(row, 2).Value = c.NroCedula;
                ws.Cell(row, 3).Value = c.Telefono;
                if (c.Mesa.HasValue)
                    ws.Cell(row, 4).Value = c.Mesa.Value;
                else
                    ws.Cell(row, 4).Value = string.Empty;
                ws.Cell(row, 5).Value = c.OperadorResponsable;
                // columna 6 (Mesa Asignada / OBS) vacía
                row++;
            }

            row++; // espacio entre locales
        }

        ws.Column(1).Width = 32;  // Nombre
        ws.Column(2).Width = 13;  // Cédula
        ws.Column(3).Width = 15;  // Teléfono
        ws.Column(4).Width = 16;  // Mesa
        ws.Column(5).Width = 25;  // Operador
        ws.Column(6).Width = 25;  // Mesa Asignada / OBS

        ws.PageSetup.PageOrientation = XLPageOrientation.Landscape;

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return stream.ToArray();
    }
}
