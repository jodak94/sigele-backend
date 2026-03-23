using Application.Reportes.DTOs;
using Application.Reportes.Interfaces;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Infrastructure.Reports;

public class CandidatosMesaPdfExporter : ICandidatosMesaExporter
{
    public string ContentType   => "application/pdf";
    public string FileExtension => "pdf";

    public byte[] Export(CandidatosMesaResult reporte)
    {
        QuestPDF.Settings.License = LicenseType.Community;

        return Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4.Landscape());
                page.Margin(1.5f, Unit.Centimetre);
                page.DefaultTextStyle(x => x.FontSize(9).FontFamily("Arial"));

                page.Header()
                    .PaddingBottom(10)
                    .Column(col =>
                    {
                        col.Item().Row(row =>
                        {
                            row.RelativeItem()
                                .Text("Candidatos a Mesa")
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

                page.Content()
                    .PaddingTop(8)
                    .Column(mainCol =>
                    {
                        foreach (var local in reporte.Locales)
                        {
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
                                    cols.RelativeColumn(3);   // Nombre del Candidato
                                    cols.ConstantColumn(65);  // Cédula
                                    cols.RelativeColumn(2);   // Teléfono
                                    cols.ConstantColumn(65);  // Vota en Mesa N°
                                    cols.RelativeColumn(2);   // Operador Responsable
                                    cols.RelativeColumn(2);   // Mesa Asignada / OBS
                                });

                                table.Header(header =>
                                {
                                    void H(string text) =>
                                        header.Cell()
                                            .Background(Colors.Grey.Lighten3)
                                            .PaddingVertical(4).PaddingHorizontal(5)
                                            .Text(text).Bold().FontSize(7.5f)
                                            .FontColor(Colors.Grey.Darken2);

                                    H("NOMBRE DEL CANDIDATO A MESA");
                                    H("CÉDULA");
                                    H("TELÉFONO");
                                    H("VOTA EN MESA N°");
                                    H("OPERADOR RESPONSABLE");
                                    H("MESA ASIGNADA / OBS");
                                });

                                foreach (var c in local.Candidatos)
                                {
                                    void D(string text) =>
                                        table.Cell()
                                            .BorderBottom(1).BorderColor(Colors.Grey.Lighten2)
                                            .PaddingVertical(7).PaddingHorizontal(5)
                                            .AlignMiddle()
                                            .Text(text).FontSize(8.5f);

                                    D(c.NombreApellido);
                                    D(c.NroCedula.ToString());
                                    D(c.Telefono);
                                    D(c.Mesa?.ToString() ?? "—");
                                    D(c.OperadorResponsable);
                                    D(""); // Mesa Asignada / OBS — vacío
                                }
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
