using Application.Reportes.DTOs;
using Application.Reportes.Interfaces;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Infrastructure.Reports;

public class ListaAsistenciaPdfExporter : IListaAsistenciaExporter
{
    public string ContentType   => "application/pdf";
    public string FileExtension => "pdf";

    public byte[] Export(ListaAsistenciaResult reporte)
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
                                .Text("Lista de Asistencia")
                                .Bold()
                                .FontSize(16);

                            row.AutoItem()
                                .AlignRight()
                                .Column(c =>
                                {
                                    c.Item().AlignRight().Text(reporte.OperadorNombre).Bold().FontSize(10);
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
                            cols.ConstantColumn(70);   // N° Cédula
                            cols.RelativeColumn(3);    // Nombre y Apellido
                            cols.ConstantColumn(80);   // Teléfono
                            cols.RelativeColumn(3);    // Local de Votación
                            cols.ConstantColumn(40);   // Mesa
                            cols.ConstantColumn(40);   // Orden
                            cols.RelativeColumn(2);    // Firma/Asistencia
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

                            HeaderCell("N° CÉDULA");
                            HeaderCell("NOMBRE Y APELLIDO");
                            HeaderCell("TELÉFONO");
                            HeaderCell("LOCAL DE VOTACIÓN");
                            HeaderCell("MESA");
                            HeaderCell("ORDEN");
                            HeaderCell("FIRMA / ASISTENCIA");
                        });

                        // Filas
                        foreach (var item in reporte.Items)
                        {
                            void DataCell(Action<IContainer> content) =>
                                table.Cell()
                                    .BorderBottom(1).BorderColor(Colors.Grey.Lighten2)
                                    .PaddingVertical(8)
                                    .PaddingHorizontal(6)
                                    .AlignMiddle()
                                    .Element(content);

                            DataCell(c => c.Text(item.NroCedula.ToString()).FontSize(9));
                            DataCell(c => c.Text(item.NombreApellido).FontSize(9));
                            DataCell(c => c.Text(item.Telefono).FontSize(9));
                            DataCell(c => c.Text(item.LocalVotacion ?? "—").FontSize(9));
                            DataCell(c => c.Text(item.Mesa?.ToString() ?? "—").FontSize(9).AlignCenter());
                            DataCell(c => c.Text(item.Orden?.ToString() ?? "—").FontSize(9).AlignCenter());
                            DataCell(_ => { }); // columna vacía para firma
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
