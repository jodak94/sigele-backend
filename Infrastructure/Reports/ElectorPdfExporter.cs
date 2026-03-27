using Application.Reportes.DTOs;
using Application.Reportes.Interfaces;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Infrastructure.Reports;

public class ElectorPdfExporter : IReportExporter
{
    public string ContentType   => "application/pdf";
    public string FileExtension => "pdf";

    public byte[] Export(ReporteElectoresResult reporte)
    {
        QuestPDF.Settings.License = LicenseType.Community;

        return Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4.Landscape());
                page.Margin(1.5f, Unit.Centimetre);
                page.DefaultTextStyle(x => x.FontSize(10).FontFamily("Arial"));

                page.Header()
                    .PaddingBottom(12)
                    .Column(col =>
                    {
                        col.Item().Row(row =>
                        {
                            row.RelativeItem()
                                .Text("Lista de Electores")
                                .Bold()
                                .FontSize(18);

                            row.AutoItem()
                                .AlignRight()
                                .Column(c =>
                                {
                                    c.Item().AlignRight().Text(reporte.OperadorNombre).Bold().FontSize(11);
                                    c.Item().AlignRight().Text(DateTime.Now.ToString("dd/MM/yyyy HH:mm")).FontSize(8).FontColor(Colors.Grey.Medium);
                                });
                        });

                        col.Item().PaddingTop(4).LineHorizontal(1).LineColor(Colors.Grey.Lighten2);
                    });

                page.Content()
                    .PaddingTop(8)
                    .Table(table =>
                    {
                        table.ColumnsDefinition(cols =>
                        {
                            cols.RelativeColumn(2);  // Nro Doc
                            cols.RelativeColumn(3);  // Nombre
                            cols.RelativeColumn(3);  // Apellido
                            cols.RelativeColumn(4);  // Local
                            cols.RelativeColumn(1);  // Mesa
                            cols.RelativeColumn(1);  // Orden
                            cols.RelativeColumn(3);  // Firma / Asistencia
                        });

                        table.Header(header =>
                        {
                            void HeaderCell(string text) =>
                                header.Cell()
                                    .Background(Colors.Grey.Lighten3)
                                    .PaddingVertical(6)
                                    .PaddingHorizontal(6)
                                    .Text(text)
                                    .Bold()
                                    .FontSize(8)
                                    .FontColor(Colors.Grey.Darken2);

                            HeaderCell("NRO DOC");
                            HeaderCell("NOMBRE");
                            HeaderCell("APELLIDO");
                            HeaderCell("LOCAL DE VOTACIÓN");
                            HeaderCell("MESA");
                            HeaderCell("ORDEN");
                            HeaderCell("FIRMA / ASISTENCIA");
                        });

                        foreach (var e in reporte.Electores)
                        {
                            void DataCell(string text, bool center = false)
                            {
                                var cell = table.Cell()
                                    .BorderBottom(1).BorderColor(Colors.Grey.Lighten2)
                                    .PaddingVertical(7)
                                    .PaddingHorizontal(6);

                                var txt = cell.Text(text).FontSize(9);
                                if (center) txt.FontColor(Colors.Grey.Darken1);
                            }

                            DataCell(e.NroDocumento.ToString());
                            DataCell(e.Nombre);
                            DataCell(e.Apellido);
                            DataCell(e.LocalVotacion ?? "—");
                            DataCell(e.Mesa?.ToString()  ?? "—", center: true);
                            DataCell(e.Orden?.ToString() ?? "—", center: true);
                            DataCell(""); // Firma / Asistencia
                        }
                    });

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
