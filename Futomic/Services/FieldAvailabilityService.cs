using Futomic.Data;
using Futomic.Models;
using Futomic.View_Models;
using Microsoft.EntityFrameworkCore;

namespace Futomic.Services
{
    //Servicio encargado de calcular la disponibilidad horaria de un campo de fútbol en una fecha concreta.
    public class FieldAvailabilityService
    {
        private readonly ApplicationDbContext _context;

        public FieldAvailabilityService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Obtiene la disponibilidad de un campo para un día específico y devuelve los slots horarios indicando si están libres u ocupados.
        public async Task<FieldAvailabilityViewModel?> GetAvailabilityAsync(
            int fieldId,
            DateTime date)
        {
            // Buscamos el campo
            var field = await _context.Fields.FindAsync(fieldId);
            if (field == null) return null;

            // Obtenemos todas las reservas activas del campo en ese día
            var reservations = await _context.Reservations
                .Where(r =>
                    r.FieldId == fieldId &&
                    r.State != EstadoReserva.Cancelada &&
                    r.DateReservation.Date == date.Date)
                .ToListAsync();

            var slots = new List<TimeSlotViewModel>();

            // Horario del campo (ejemplo: 09:00 - 23:00)
            var opening = new TimeSpan(9, 0, 0);
            var closing = new TimeSpan(23, 0, 0);

            // Generamos slots de 1 hora
            for (var time = opening; time < closing; time += TimeSpan.FromHours(1))
            {
                // Convertimos el TimeSpan en DateTime real
                var slotStart = date.Date.Add(time); // DateTime ✔
                var slotEnd = slotStart.AddHours(1); // DateTime ✔

                // Comprobamos si el slot se solapa con alguna reserva existente
                bool overlaps = reservations.Any(r =>
                {
                    var resStart = r.DateReservation;
                    var resEnd = r.DateReservation.AddMinutes(r.Duration);

                    return slotStart < resEnd && slotEnd > resStart;
                });

                // Añadimos el slot indicando si está disponible
                slots.Add(new TimeSlotViewModel
                {
                    Start = slotStart,
                    End = slotEnd,
                    IsAvailable = !overlaps && slotStart > DateTime.Now
                });
            }

            // Añadimos el slot indicando si está disponible
            return new FieldAvailabilityViewModel
            {
                FieldId = field.FieldId,
                FieldName = field.Name!,
                Location = field.Location!,
                SelectedDate = date,
                TimeSlots = slots
            };
        }
    }

}
