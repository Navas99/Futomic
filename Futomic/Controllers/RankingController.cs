using Futomic.Data;
using Futomic.Models;
using Futomic.Services;
using Futomic.View_Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Futomic.Controllers
{
    // Controlador encargado del ranking de equipos y la gestión de partidos solo admin
    [Authorize]
    public class RankingController : Controller
    {
        private readonly IRankingService _rankingService;

        public RankingController(IRankingService rankingService)
        {
            _rankingService = rankingService;
        }

        public async Task<IActionResult> Index(LevelTeam? level)
        {
            var model = await _rankingService.GetRankingAsync(level);

            //filtrar por nivel
            ViewBag.Levels = Enum.GetValues(typeof(LevelTeam));
            ViewBag.SelectedLevel = level;

            return View(model);
        }

        //registrar nuevo partido solo admin
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RegisterMatch(int? id)
        {
            return View(await _rankingService.GetRegisterMatchAsync(id));
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RegisterMatch(RegisterMatchViewModel vm)
        {
            //actualiza ranking
            await _rankingService.SaveMatchAsync(vm);
            return RedirectToAction(nameof(Index));
        }

        //elimina y actualiza ranking
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteMatch(int id)
        {
            await _rankingService.DeleteMatchAsync(id);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Results()
        {
            return View(await _rankingService.GetLastResultsAsync());
        }
    }

}
