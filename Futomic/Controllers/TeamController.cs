using Futomic.Data;
using Futomic.Models;
using Futomic.Services;
using Futomic.View_Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Futomic.Controllers
{
    // Controlador encargado de la gestión de equipos:
    //listado, detalle, creación, edición(admin), eliminación(admin) y operaciones del usuario con su equipo.
    [Authorize]
    public class TeamController : Controller
    {
        private readonly ITeamService _teamService;
        private readonly UserManager<User> _userManager;
        private readonly IWebHostEnvironment _env;

        public TeamController(
            ITeamService teamService,
            UserManager<User> userManager,
            IWebHostEnvironment env)
        {
            _teamService = teamService;
            _userManager = userManager;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _teamService.GetTeamsAsync());
        }

        public async Task<IActionResult> Details(int id)
        {
            var team = await _teamService.GetTeamDetailsAsync(id);
            if (team == null) return NotFound();

            return View(team);
        }

        // Permite a un usuario unirse a un equipo y controla que no pertenezca ya a otro equipo.
        [HttpPost]
        public async Task<IActionResult> Join(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Unauthorized();

            try
            {
                await _teamService.JoinTeamAsync(id, user.Id);
                TempData["success"] = "Te has unido al equipo correctamente.";
            }
            catch (InvalidOperationException ex)
            {
                // Error controlado: usuario ya pertenece a un equipo
                TempData["error"] = ex.Message;
            }
            catch (Exception)
            {
                TempData["error"] = "No se pudo unir al equipo.";
            }

            return RedirectToAction(nameof(Index));
        }

        // Formulario de creación de equipo, carga los niveles disponibles desde el enum.
        public IActionResult Create()
        {
            ViewBag.Levels = Enum.GetValues(typeof(LevelTeam))
                .Cast<LevelTeam>()
                .Select(l => new SelectListItem
                {
                    Value = ((int)l).ToString(),
                    Text = l.ToString()
                }).ToList();

            return View(new TeamViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TeamViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                // Volver a cargar los niveles si hay error de validación
                ViewBag.Levels = Enum.GetValues(typeof(LevelTeam))
                    .Cast<LevelTeam>()
                    .Select(l => new SelectListItem
                    {
                        Value = ((int)l).ToString(),
                        Text = l.ToString()
                    }).ToList();

                return View(vm);
            }

            await _teamService.CreateTeamAsync(vm, _env);
            return RedirectToAction(nameof(Index));
        }


        //edición de equipo, solo accesible por admin.
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var vm = await _teamService.GetTeamForEditAsync(id);
            if (vm == null) return NotFound();
            return View(vm);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(TeamEditViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            await _teamService.UpdateTeamAsync(vm, _env);
            return RedirectToAction(nameof(Index));
        }


        // Elimina un equipo y todas sus dependencias solo accesible admin
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _teamService.DeleteTeamAsync(id);
                TempData["success"] = "Equipo eliminado correctamente.";
            }
            catch (Exception ex)
            {
                TempData["error"] = $"No se pudo eliminar el equipo: {ex.Message}";
            }

            return RedirectToAction(nameof(Index));
        }


        // Vista "Mi Equipo" del usuario autenticado y muestra información completa del equipo,jugadores, partidos y estadísticas.
        public async Task<IActionResult> MyTeam()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Unauthorized();

            var vm = await _teamService.GetMyTeamAsync(user.Id);

            if (vm == null)
            {
                TempData["error"] = "No perteneces a ningún equipo.";
                return RedirectToAction("Index");
            }

            return View(vm);
        }
    }


}
