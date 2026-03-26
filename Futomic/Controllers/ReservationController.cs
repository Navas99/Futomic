using Futomic.Data;
using Futomic.Models;
using Futomic.Services;
using Futomic.View_Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Futomic.Controllers
{
    // Controlador encargado de la creación, confirmación y consulta de reservas de campos.
    [Authorize]
    public class ReservationController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly ReservationService _reservationService;

        public ReservationController(ApplicationDbContext context, UserManager<User> userManager, ReservationService reservationService)
        {
            _context = context;
            _userManager = userManager;
            _reservationService = reservationService;
        }


        // Muestra el formulario de creación de reserva con la fecha y el campo seleccionados desde el calendario.
        public async Task<IActionResult> Create(int fieldId, DateTime date)
        {
            var field = await _context.Fields.FindAsync(fieldId);
            if (field == null) return NotFound();

            var vm = new ReservationCreateViewModel
            {
                FieldId = fieldId,
                FieldName = field.Name!,
                DateReservation = date,
                Duration = 60 // default
            };

            return View(vm);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ReservationCreateViewModel model)
        {
            //Validamos modelo
            if (!ModelState.IsValid)
                return View(model);

            //Usuario autenticado
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            //El usuario debe pertenecer a un equipo
            if (user.TeamId == null)
            {
                ModelState.AddModelError("", "Debes pertenecer a un equipo para reservar.");
                return View(model);
            }

            // Recalcular precio en servidor
            model.Price = model.Duration switch
            {
                60 => 60,
                120 => 100,
                240 => 200,
                _ => throw new InvalidOperationException("Duración inválida")
            };

            var start = model.DateReservation;
            var end = start.AddMinutes(model.Duration);

            // Validar solapamiento de reservas
            bool overlap = await _context.Reservations.AnyAsync(r =>
                r.FieldId == model.FieldId &&
                r.State != EstadoReserva.Cancelada &&
                start < r.DateReservation.AddMinutes(r.Duration) &&
                end > r.DateReservation
            );

            if (overlap)
            {
                ModelState.AddModelError("", "Ese campo ya está reservado en ese horario.");
                return View(model);
            }

            // Validar que el equipo no tenga otra reserva activa
            bool hasActiveReservation = await _context.Reservations.AnyAsync(r =>
                r.TeamId == user.TeamId &&
                r.DateReservation.Date == model.DateReservation.Date &&
                r.State == EstadoReserva.Pendiente
            );

            if (hasActiveReservation)
            {
                ModelState.AddModelError("", "Tu equipo ya tiene una reserva activa ese día.");
                return View(model);
            }

            //Creamos la reserva
            var reservation = new Reservation
            {
                FieldId = model.FieldId,
                TeamId = user.TeamId!.Value,
                DateReservation = model.DateReservation,
                Duration = model.Duration,
                Paymen = model.Paymen,
                Price = model.Price,
                State = EstadoReserva.Pendiente
            };

            _context.Reservations.Add(reservation);
            await _context.SaveChangesAsync();

            return RedirectToAction("Confirm", new { id = reservation.ReservationId });
        }

        // Vista de confirmación de la reserva creada.
        public async Task<IActionResult> Confirm(int id)
        {
            var reservation = await _context.Reservations
                .Include(r => r.Field)
                .Include(r => r.Team)
                .FirstOrDefaultAsync(r => r.ReservationId == id);

            if (reservation == null) return NotFound();

            return View(reservation);
        }

        //Genera y descarga el ticket de reserva en PDF.
        public async Task<IActionResult> DownloadPdf(int id)
        {
            var reservation = await _context.Reservations
                .Include(r => r.Field)
                .Include(r => r.Team)
                .FirstOrDefaultAsync(r => r.ReservationId == id);

            if (reservation == null)
                return NotFound();

            byte[] pdfBytes = PdfService.GenerateReservationPdf(reservation);

            return File(pdfBytes, "application/pdf", $"reserva_{id}.pdf");
        }

        // Vista de calendario para seleccionar fecha y hora.
        public async Task<IActionResult> SelectSlot(int fieldId, DateTime? date)
        {
            var selectedDate = date ?? DateTime.Today;

            var vm = await _reservationService.GetCalendarAsync(fieldId, selectedDate);

            return View(vm);
        }

        // Muestra las reservas activas del usuario autenticado.
        public async Task<IActionResult> MyReservations()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            var reservations = await _reservationService.GetUserReservationsAsync(user.Id);

            return View(reservations);
        }


    }
}