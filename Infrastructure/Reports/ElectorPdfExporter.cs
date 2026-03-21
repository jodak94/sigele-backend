using Application.Reportes.DTOs;
using Application.Reportes.Interfaces;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Infrastructure.Reports;

public class ElectorPdfExporter : IReportExporter
{
    public string ContentType => "application/pdf";
    public string FileExtension => "pdf";

    public byte[] Export(ReporteElectoresResult reporte)
    {
        QuestPDF.Settings.License = LicenseType.Community;

        return Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(1.5f, Unit.Centimetre);
                page.DefaultTextStyle(x => x.FontSize(10).FontFamily("Arial"));

                page.Header()
                    .PaddingBottom(12)
                    .Column(col =>
                    {
                        col.Item().Row(row =>
                        {
                            row.RelativeItem()
                                .Text("Mi Lista de Electores")
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
                            cols.RelativeColumn(5);  // ELECTOR
                            cols.RelativeColumn(2);  // CONTACTO
                            cols.RelativeColumn(2);  // ETIQUETAS
                        });

                        // Encabezado de tabla
                        table.Header(header =>
                        {
                            void HeaderCell(string text) =>
                                header.Cell()
                                    .Background(Colors.Grey.Lighten3)
                                    .PaddingVertical(6)
                                    .PaddingHorizontal(8)
                                    .Text(text)
                                    .Bold()
                                    .FontSize(8)
                                    .FontColor(Colors.Grey.Darken2);

                            HeaderCell("ELECTOR");
                            HeaderCell("CONTACTO");
                            HeaderCell("ETIQUETAS");
                        });

                        // Filas de datos
                        foreach (var e in reporte.Electores)
                        {
                            var nombreCompleto = $"{e.Apellido} {e.Nombre}".Trim().ToUpperInvariant();

                            // ELECTOR
                            table.Cell()
                                .BorderBottom(1).BorderColor(Colors.Grey.Lighten2)
                                .PaddingVertical(10)
                                .PaddingHorizontal(8)
                                .Column(col =>
                                {
                                    col.Item().Text(nombreCompleto).Bold().FontSize(10);
                                    col.Item().Text($"CI: {e.NroDocumento}").FontSize(9).FontColor(Colors.Grey.Darken1);
                                    if (!string.IsNullOrWhiteSpace(e.Direccion))
                                    {
                                        col.Item().PaddingTop(2).Text(e.Direccion).FontSize(9).FontColor("#e53e3e");
                                    }
                                });

                            // CONTACTO
                            table.Cell()
                                .BorderBottom(1).BorderColor(Colors.Grey.Lighten2)
                                .PaddingVertical(10)
                                .PaddingHorizontal(8)
                                .AlignMiddle()
                                .Text(e.NroTelefono)
                                .FontSize(9);

                            // ETIQUETAS
                            table.Cell()
                                .BorderBottom(1).BorderColor(Colors.Grey.Lighten2)
                                .PaddingVertical(10)
                                .PaddingHorizontal(8)
                                .AlignMiddle()
                                .Column(col =>
                                {
                                    if (e.MiembroMesa)
                                        col.Item().Text("Mesa").FontSize(9).FontColor(Colors.Grey.Medium);
                                    if (e.RequiereTransporte)
                                        col.Item().Text("Transporte").FontSize(9).FontColor(Colors.Grey.Medium);
                                });
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
