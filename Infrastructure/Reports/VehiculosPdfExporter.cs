using Application.Reportes.DTOs;
using Application.Reportes.Interfaces;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Infrastructure.Reports;

public class VehiculosPdfExporter : IVehiculosExporter
{
    public string ContentType   => "application/pdf";
    public string FileExtension => "pdf";

    public byte[] Export(VehiculosReporteData reporte)
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
                                .Text("Reporte de Vehículos")
                                .Bold()
                                .FontSize(16);

                            row.AutoItem()
                                .AlignRight()
                                .Column(c =>
                                {
                                    c.Item().AlignRight()
                                        .Text($"Total: {reporte.Total} vehículo(s)")
                                        .Bold()
                                        .FontSize(10);
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
                            cols.RelativeColumn(3);   // Nombre del Dueño
                            cols.RelativeColumn(2);   // Teléfono
                            cols.RelativeColumn(1);   // Capacidad
                            cols.RelativeColumn(2);   // Monto Alquiler
                            cols.RelativeColumn(3);   // Operador
                            cols.RelativeColumn(3);   // Observación
                        });

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

                            HeaderCell("NOMBRE DEL DUEÑO");
                            HeaderCell("TELÉFONO");
                            HeaderCell("CAPACIDAD");
                            HeaderCell("MONTO ALQUILER");
                            HeaderCell("OPERADOR");
                            HeaderCell("OBSERVACIÓN");
                        });

                        foreach (var item in reporte.Vehiculos)
                        {
                            void DataCell(string text) =>
                                table.Cell()
                                    .BorderBottom(1).BorderColor(Colors.Grey.Lighten2)
                                    .PaddingVertical(8)
                                    .PaddingHorizontal(6)
                                    .AlignMiddle()
                                    .Text(text)
                                    .FontSize(9)
                                    .WrapAnywhere();

                            DataCell(item.NombreDueno);
                            DataCell(item.TelefonoDueno);
                            DataCell(item.Capacidad.ToString());
                            DataCell(item.MontoAlquiler.ToString("N0"));
                            DataCell(item.OperadorNombre ?? "—");
                            DataCell(item.Observacion ?? "—");
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
