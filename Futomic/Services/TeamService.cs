using Futomic.Data;
using Futomic.Models;
using Futomic.View_Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Futomic.Services
{
    // Servicio encargado de la gestión completa de equipos:
    //creación, edición, unión de usuarios, eliminación y vista "Mi Equipo".
    public class TeamService : ITeamService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public TeamService(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        //Obtiene todos los equipos registrados
        public async Task<List<Team>> GetTeamsAsync()
        {
            return await _context.Teams.ToListAsync();
        }

        //Obtiene detalles de un equipo concreto
        public async Task<Team?> GetTeamDetailsAsync(int teamId)
        {
            return await _context.Teams
                .Include(t => t.Users)
                .FirstOrDefaultAsync(t => t.TeamId == teamId);
        }

        // Asocia un usuario a un equipo existente.
        public async Task JoinTeamAsync(int teamId, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                throw new Exception("Usuario no encontrado.");

            //Un user solo puede estar en un equipo
            if (user.TeamId != null)
                throw new InvalidOperationException("Ya perteneces a un equipo.");

            var team = await _context.Teams.FindAsync(teamId);
            if (team == null)
                throw new Exception("El equipo no existe.");

            user.TeamId = team.TeamId;
            _context.Users.Update(user);

            await _context.SaveChangesAsync();
        }

        //Crea un nuevo equipo y su ranking inicial
        public async Task CreateTeamAsync(TeamViewModel vm, IWebHostEnvironment env)
        {
            string? logoPath = null;

            //guarda logo si existe
            if (vm.LogoFile != null)
            {
                var folder = Path.Combine(env.WebRootPath, "logos");
                Directory.CreateDirectory(folder);

                var fileName = Guid.NewGuid() + Path.GetExtension(vm.LogoFile.FileName);
                var path = Path.Combine(folder, fileName);

                using var stream = new FileStream(path, FileMode.Create);
                await vm.LogoFile.CopyToAsync(stream);

                logoPath = "/logos/" + fileName;
            }

            var team = new Team
            {
                Name = vm.Name,
                Captain = vm.Captain,
                LogoUrl = logoPath,
                Level = (LevelTeam)vm.Level!,
                CreationDate = DateTime.Now
            };

            _context.Teams.Add(team);
            await _context.SaveChangesAsync();

            // Ranking inicial
            var ranking = new Ranking
            {
                TeamId = team.TeamId,
                Level = team.Level,
                Points = 0,
                LastMatchResult = "N/A"
            };

            _context.Rankings.Add(ranking);
            await _context.SaveChangesAsync();
        }
        //Editaar equipo
        public async Task<TeamEditViewModel?> GetTeamForEditAsync(int teamId)
        {
            var team = await _context.Teams.FindAsync(teamId);
            if (team == null) return null;

            return new TeamEditViewModel
            {
                TeamId = team.TeamId,
                Name = team.Name!,
                Captain = team.Captain!,
                Level = team.Level,
                ExistingLogoUrl = team.LogoUrl
            };
        }

        //Actualiza los datos de un equipo existente.
        public async Task UpdateTeamAsync(TeamEditViewModel vm, IWebHostEnvironment env)
        {
            var team = await _context.Teams.FindAsync(vm.TeamId);
            if (team == null) return;

            team.Name = vm.Name;
            team.Captain = vm.Captain;
            team.Level = vm.Level;

            //Actualizar logo si se sube uno nuevo
            if (vm.LogoFile != null)
            {
                var folder = Path.Combine(env.WebRootPath, "logos");
                Directory.CreateDirectory(folder);

                var fileName = Guid.NewGuid() + Path.GetExtension(vm.LogoFile.FileName);
                var path = Path.Combine(folder, fileName);

                using var stream = new FileStream(path, FileMode.Create);
                await vm.LogoFile.CopyToAsync(stream);

                team.LogoUrl = "/logos/" + fileName;
            }

            _context.Teams.Update(team);
            await _context.SaveChangesAsync();
        }


        //Elimina un equipo junto con todas sus dependencias (partidos, reservas, ranking y usuarios asociados).
        public async Task DeleteTeamAsync(int teamId)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var team = await _context.Teams
                    .Include(t => t.MatchesAsTeamA)
                    .Include(t => t.MatchesAsTeamB)
                    .Include(t => t.Reservations)
                    .Include(t => t.Users)
                    .Include(t => t.Ranking)
                    .FirstOrDefaultAsync(t => t.TeamId == teamId);

                if (team == null)
                    throw new Exception("El equipo no existe.");

                //Eliminar partidos donde sea TeamA
                _context.Matches.RemoveRange(team.MatchesAsTeamA);

                //Eliminar partidos donde sea TeamB
                _context.Matches.RemoveRange(team.MatchesAsTeamB);

                //Eliminar reservas asociadas
                _context.Reservations.RemoveRange(team.Reservations);

                //Desvincular usuarios del equipo
                foreach (var user in team.Users)
                {
                    user.TeamId = null;
                }

                //Eliminar ranking
                if (team.Ranking != null)
                    _context.Rankings.Remove(team.Ranking);

                //Finalmente eliminar el equipo
                _context.Teams.Remove(team);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
        //Obtiene la información completa del equipo del usuario autenticado.
        public async Task<MyTeamViewModel?> GetMyTeamAsync(string userId)
        {
            var user = await _context.Users
                .Include(u => u.Team)
                    .ThenInclude(t => t!.Users)
                .Include(u => u.Team)
                    .ThenInclude(t => t!.MatchesAsTeamA)
                        .ThenInclude(m => m.TeamB)
                .Include(u => u.Team)
                    .ThenInclude(t => t!.MatchesAsTeamB)
                        .ThenInclude(m => m.TeamA)
                .Include(u => u.Team)
                    .ThenInclude(t => t!.Ranking)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user?.Team == null)
                return null;

            var team = user.Team;

            //Partidos jugados (vista del equipo)
            var matches = team.MatchesAsTeamA
                .Select(m => new TeamMatchViewModel
                {
                    Rival = m.TeamB.Name!,
                    Result = m.Result, // Victoria / Empate / Derrota (TeamA)
                    PlayedAt = m.PlayedAt
                })
                .Concat(team.MatchesAsTeamB.Select(m => new TeamMatchViewModel
                {
                    Rival = m.TeamA.Name!,
                    Result = m.Result switch
                    {
                        "Victoria" => "Derrota",
                        "Derrota" => "Victoria",
                        _ => "Empate"
                    },
                    PlayedAt = m.PlayedAt
                }))
                .OrderByDescending(m => m.PlayedAt)
                .ToList();

            //Estadísticas
            int wins = matches.Count(m => m.Result == "Victoria");
            int draws = matches.Count(m => m.Result == "Empate");
            int losses = matches.Count(m => m.Result == "Derrota");

            return new MyTeamViewModel
            {
                TeamId = team.TeamId,
                Name = team.Name!,
                LogoUrl = team.LogoUrl,
                Level = team.Level,
                CreationDate = team.CreationDate,

                Players = team.Users.Select(u => new PlayerRowViewModel
                {
                    Name = $"{u.Name} {u.LastName}",
                    Position = u.Position?.ToString() ?? string.Empty
                }).ToList(),

                Matches = matches,

                Stats = new TeamStatsViewModel
                {
                    MatchesPlayed = matches.Count,
                    Wins = wins,
                    Draws = draws,
                    Losses = losses,
                    Points = team.Ranking?.Points ?? 0,
                    Form = team.Ranking?.CurrentStreak ?? ""
                }
            };
        }

    }

}

