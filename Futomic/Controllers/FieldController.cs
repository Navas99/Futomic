using Futomic.Data;
using Futomic.Models;
using Futomic.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;



namespace Futomic.Controllers
{
    [Authorize]
    public class FieldController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly FieldAvailabilityService _availabilityService;

        public FieldController(ApplicationDbContext context, FieldAvailabilityService availabilityService)
        {
            _context = context;
            _availabilityService = availabilityService;
        }

        // Vista de búsqueda de campos por ubicación o PlusCode.
        public async Task<IActionResult> Search(string location)
        {
            // Si no se introduce ninguna ubicación,
            // devolvemos la vista vacía sin lanzar errores
            if (string.IsNullOrWhiteSpace(location))
                return View(new List<Field>());

            location = location.ToLower();

            // Buscamos campos cuya localización o PlusCode
            // contengan el texto introducido
            var fields = await _context.Fields
                .Where(f => f.Location!.ToLower().Contains(location) ||
                            (f.PlusCode != null && f.PlusCode.ToLower().Contains(location)))
                .ToListAsync();

            return View(fields);
        }

        // Map con coordenadas precargadas
        [Authorize]
        public async Task<IActionResult> Map()
        {
            var fields = await _context.Fields.ToListAsync();
            return View(fields);
        }

        // Muestra la disponibilidad horaria de un campo en una fecha concreta.
        public async Task<IActionResult> Availability(int id, DateTime? date)
        {
            // Si no se selecciona fecha, se usa el día actual
            var selectedDate = date ?? DateTime.Today;

            // Delegamos toda la lógica de negocio al servicio
            var model = await _availabilityService
                .GetAvailabilityAsync(id, selectedDate);

            // Si el campo no existe, devolvemos 404
            if (model == null)
                return NotFound();

            return View(model);
        }
    }

}

