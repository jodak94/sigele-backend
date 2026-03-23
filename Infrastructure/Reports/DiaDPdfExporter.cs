using Application.Reportes.DTOs;
using Application.Reportes.Interfaces;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Infrastructure.Reports;

public class DiaDPdfExporter : IDiaDExporter
{
    public string ContentType   => "application/pdf";
    public string FileExtension => "pdf";

    public byte[] Export(DiaDResult reporte)
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
                                .Text("Reporte Día D")
                                .Bold()
                                .FontSize(16);

                            row.AutoItem()
                                .AlignRight()
                                .Text(DateTime.Now.ToString("dd/MM/yyyy HH:mm"))
                                .FontSize(8)
                                .FontColor(Colors.Grey.Medium);
                        });

                        col.Item().PaddingTop(4).LineHorizontal(1).LineColor(Colors.Grey.Lighten2);
                    });

                // ── Content ───────────────────────────────────────────────
                page.Content()
                    .PaddingTop(8)
                    .Column(mainCol =>
                    {
                        foreach (var local in reporte.Locales)
                        {
                            // Encabezado del local
                            mainCol.Item()
                                .PaddingTop(14)
                                .PaddingBottom(4)
                                .Row(row =>
                                {
                                    row.AutoItem()
                                        .Text("Local: ")
                                        .FontSize(10)
                                        .FontColor(Colors.Grey.Medium);
                                    row.RelativeItem()
                                        .Text(local.LocalVotacion)
                                        .Bold()
                                        .FontSize(10);
                                });

                            mainCol.Item().Table(table =>
                            {
                                table.ColumnsDefinition(cols =>
                                {
                                    cols.ConstantColumn(32);  // Mesa
                                    cols.ConstantColumn(32);  // Orden
                                    cols.ConstantColumn(62);  // Cédula
                                    cols.RelativeColumn(3);   // Elector
                                    cols.RelativeColumn(2);   // Teléfono
                                    cols.RelativeColumn(3);   // Dir. Recogida
                                    cols.ConstantColumn(50);  // Transporte
                                    cols.RelativeColumn(2);   // Operador
                                });

                                table.Header(header =>
                                {
                                    void H(string text) =>
                                        header.Cell()
                                            .Background(Colors.Grey.Lighten3)
                                            .PaddingVertical(4)
                                            .PaddingHorizontal(4)
                                            .Text(text).Bold().FontSize(7.5f)
                                            .FontColor(Colors.Grey.Darken2);

                                    H("MESA"); H("ORDEN"); H("CÉDULA"); H("ELECTOR");
                                    H("TELÉFONO"); H("DIR. RECOGIDA"); H("TRANSPORTE"); H("OPERADOR");
                                });

                                foreach (var e in local.Electores)
                                {
                                    void D(string text) =>
                                        table.Cell()
                                            .BorderBottom(1).BorderColor(Colors.Grey.Lighten2)
                                            .PaddingVertical(6).PaddingHorizontal(4)
                                            .AlignMiddle()
                                            .Text(text).FontSize(8.5f);

                                    D(e.Mesa?.ToString()  ?? "—");
                                    D(e.Orden?.ToString() ?? "—");
                                    D(e.NroCedula.ToString());
                                    D(e.NombreApellido);
                                    D(e.Telefono);
                                    D(string.IsNullOrWhiteSpace(e.DireccionRecogida) ? "—" : e.DireccionRecogida);
                                    D(e.RequiereTransporte ? "Sí" : "No");
                                    D(e.OperadorResponsable);
                                }
                            });
                        }
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
