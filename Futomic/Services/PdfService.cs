using Futomic.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Futomic.Services
{
    public static class PdfService
    {

        public static byte[] GenerateReservationPdf(Reservation r)
        {
            QuestPDF.Settings.License = LicenseType.Community;

            var doc = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(30);
                    page.DefaultTextStyle(TextStyle.Default.FontSize(14));

                    page.Content().Column(col =>
                    {
                        col.Item().Text("Confirmación de Reserva").FontSize(24).Bold();
                        col.Item().Text($"Campo: {r.Field!.Name}");
                        col.Item().Text($"Ubicación: {r.Field.Location}");
                        col.Item().Text($"Equipo: {r.Team!.Name}");
                        col.Item().Text($"Fecha: {r.DateReservation}");
                        col.Item().Text($"Duración: {r.Duration} min");
                        col.Item().Text($"Precio: {r.Price} €");
                    });
                });
            });

            return doc.GeneratePdf();
        }
    }
}
