using Application.Reportes.DTOs;
using Application.Reportes.Interfaces;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Infrastructure.Reports;

public class ResumenOperadoresPdfExporter : IResumenOperadoresExporter
{
    public string ContentType   => "application/pdf";
    public string FileExtension => "pdf";

    public byte[] Export(ResumenOperadoresData reporte)
    {
        QuestPDF.Settings.License = LicenseType.Community;

        return Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4.Landscape());
                page.Margin(1.5f, Unit.Centimetre);
                page.DefaultTextStyle(x => x.FontSize(9).FontFamily("Arial"));

                // ── Header ────────────────────────────────────────────────
                page.Header()
                    .PaddingBottom(10)
                    .Column(col =>
                    {
                        col.Item().Row(row =>
                        {
                            row.RelativeItem()
                                .Text("Resumen de Operadores")
                                .Bold()
                                .FontSize(16);

                            row.AutoItem()
                                .AlignRight()
                                .Column(c =>
                                {
                                    if (reporte.Coordinador is not null)
                                        c.Item().AlignRight().Text(reporte.Coordinador.Nombre).Bold().FontSize(10);
                                    c.Item().AlignRight()
                                        .Text(DateTime.Now.ToString("dd/MM/yyyy HH:mm"))
                                        .FontSize(8)
                                        .FontColor(Colors.Grey.Medium);
                                });
                        });

                        col.Item().PaddingTop(4).LineHorizontal(1).LineColor(Colors.Grey.Lighten2);
                    });

                // ── Content ───────────────────────────────────────────────
                page.Content()
                    .PaddingTop(8)
                    .Table(table =>
                    {
                        table.ColumnsDefinition(cols =>
                        {
                            cols.RelativeColumn(4);   // Nombre del Operador
                            cols.RelativeColumn(2);   // Teléfono
                            cols.RelativeColumn(2);   // Electores Captados
                            cols.RelativeColumn(2);   // Miembros de Mesa Disp.
                            cols.RelativeColumn(2);   // Req. Transporte
                        });

                        // Encabezado
                        table.Header(header =>
                        {
                            void HeaderCell(string text) =>
                                header.Cell()
                                    .Background(Colors.Grey.Lighten3)
                                    .PaddingVertical(5)
                                    .PaddingHorizontal(6)
                                    .Text(text)
                                    .Bold()
                                    .FontSize(8)
                                    .FontColor(Colors.Grey.Darken2);

                            HeaderCell("NOMBRE DEL OPERADOR");
                            HeaderCell("TELÉFONO");
                            HeaderCell("ELECTORES CAPTADOS");
                            HeaderCell("MIEMBROS DE MESA DISP.");
                            HeaderCell("REQ. TRANSPORTE");
                        });

                        // Filas de datos
                        foreach (var item in reporte.Operadores)
                        {
                            void DataCell(string text, bool alignRight = false) =>
                                table.Cell()
                                    .BorderBottom(1).BorderColor(Colors.Grey.Lighten2)
                                    .PaddingVertical(8)
                                    .PaddingHorizontal(6)
                                    .AlignMiddle()
                                    .Text(text)
                                    .FontSize(9)
                                    .WrapAnywhere();

                            DataCell(item.NombreOperador);
                            DataCell(item.Telefono);
                            DataCell(item.ElectoresCaptados.ToString());
                            DataCell(item.MiembrosMesaDisp.ToString());
                            DataCell(item.ReqTransporte.ToString());
                        }

                        // Fila de totales
                        void TotalCell(string text) =>
                            table.Cell()
                                .Background(Colors.Grey.Lighten3)
                                .PaddingVertical(7)
                                .PaddingHorizontal(6)
                                .AlignMiddle()
                                .Text(text)
                                .Bold()
                                .FontSize(9);

                        TotalCell("TOTALES");
                        TotalCell("");
                        TotalCell(reporte.Totales.ElectoresCaptados.ToString());
                        TotalCell(reporte.Totales.MiembrosMesaDisp.ToString());
                        TotalCell(reporte.Totales.ReqTransporte.ToString());
                    });

                // ── Footer ────────────────────────────────────────────────
                page.Footer()
                    .AlignCenter()
                    .Text(x =>
                    {
                        x.Span("Página ").FontSize(8).FontColor(Colors.Grey.Medium);
                        x.CurrentPageNumber().FontSize(8).FontColor(Colors.Grey.Medium);
                        x.Span(" de ").FontSize(8).FontColor(Colors.Grey.Medium);
                        x.TotalPages().FontSize(8).FontColor(Colors.Grey.Medium);
                    });
            });
        }).GeneratePdf();
    }
}
