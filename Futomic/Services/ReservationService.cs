using Futomic.Data;
using Futomic.Models;
using Futomic.View_Models;
using Microsoft.EntityFrameworkCore;

namespace Futomic.Services
{
    // Servicio encargado de la lógica relacionada con las reservas:
    // - Generación del calendario de disponibilidad
    // - Obtención de reservas del usuario
    public class ReservationService
    {
        private readonly ApplicationDbContext _context;

        public ReservationService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Genera el calendario de disponibilidad de un campo para una fecha concreta. Devuelve slots horarios indicando si están disponibles o no.
        public async Task<ReservationCalendarViewModel> GetCalendarAsync(int fieldId, DateTime date)
        {
            var field = await _context.Fields.FindAsync(fieldId)
                ?? throw new Exception("Campo no encontrado");

            //Horario default
            DateTime dayStart = date.Date.AddHours(9);
            DateTime dayEnd = date.Date.AddHours(23);
            var slotMinutes = 60;

            //Obtener reservas activas del día
            var reservations = await _context.Reservations
                .Where(r =>
                    r.FieldId == fieldId &&
                    r.DateReservation.Date == date.Date &&
                    r.State != EstadoReserva.Cancelada)
                .ToListAsync();

            var slots = new List<TimeSlotViewModel>();

            //Generar slots de una hora
            for (DateTime time = dayStart; time < dayEnd; time = time.AddMinutes(slotMinutes))
            {
                DateTime end = time.AddMinutes(slotMinutes);

                //comprobar solapamiento con reservas existentes
                bool overlaps = reservations.Any(r =>
                    time < r.DateReservation.AddMinutes(r.Duration) &&
                    end > r.DateReservation);

                slots.Add(new TimeSlotViewModel
                {
                    Start = time,
                    End = end,
                    // Solo disponible si no hay solape y es una hora futura
                    IsAvailable = !overlaps && time > DateTime.Now
                });
            }

            return new ReservationCalendarViewModel
            {
                FieldId = fieldId,
                FieldName = field.Name!,
                SelectedDate = date,
                Slots = slots
            };
        }

        // Obtiene las reservas activas del usuario autenticado a través del equipo al que pertenece.
        public async Task<List<MyReservationsViewModel>> GetUserReservationsAsync(string userId)
        {
            var user = await _context.Users
                .Include(u => u.Team)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user?.Team == null)
                return new List<MyReservationsViewModel>();

            //devolver reservas activas del equipo
            return await _context.Reservations
                .Include(r => r.Field)
                .Include(r => r.Team)
                .Where(r => r.TeamId == user.TeamId && r.State != EstadoReserva.Cancelada)
                .OrderByDescending(r => r.DateReservation)
                .Select(r => new MyReservationsViewModel
                {
                    ReservationId = r.ReservationId,
                    FieldName = r.Field!.Name!,
                    Location = r.Field.Location!,
                    DateReservation = r.DateReservation,
                    Duration = r.Duration,
                    Price = r.Price,
                    State = r.State,
                    TeamName = r.Team!.Name!
                })
                .ToListAsync();
        }

    }
}
