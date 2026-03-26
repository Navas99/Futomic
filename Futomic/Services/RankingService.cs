using Futomic.Data;
using Futomic.Models;
using Futomic.View_Models;
using Microsoft.EntityFrameworkCore;

namespace Futomic.Services
{
    public class RankingService : IRankingService
    {
        private readonly ApplicationDbContext _context;

        public RankingService(ApplicationDbContext context)
        {
            _context = context;
        }

        //Obtiene el ranking de equipos, Permite filtrar opcionalmente por nivel.
        public async Task<List<RankingRowViewModel>> GetRankingAsync(LevelTeam? level)
        {
            var query = _context.Rankings
                .Include(r => r.Team)
                .AsQueryable();

            //APLICAR FILTRO SOLO SI HAY NIVEL
            if (level.HasValue)
            {
                query = query.Where(r => r.Team!.Level == level.Value);
            }

            // Ordenar por puntos y mapear a ViewModel
            return await query
                .OrderByDescending(r => r.Points)
                .Select(r => new RankingRowViewModel
                {
                    
                    TeamId = r.TeamId,
                    TeamName = r.Team!.Name!,
                    TeamLogoUrl = r.Team.LogoUrl,
                    Level = r.Team.Level,
                    Points = r.Points,
                    LastMatchResult = r.LastMatchResult!,
                    CurrentStreak = r.CurrentStreak ?? ""
                })
                .ToListAsync();
        }

        // Devuelve el ViewModel necesario para registrar o editar un partido.
        //Si no se pasa matchId, se prepara para crear uno nuevo.
        public async Task<RegisterMatchViewModel> GetRegisterMatchAsync(int? matchId)
        {
            var teams = await _context.Teams.ToListAsync();

            if (matchId == null)
                return new RegisterMatchViewModel { Teams = teams };

            var match = await _context.Matches.FindAsync(matchId.Value);

            return new RegisterMatchViewModel
            {
                MatchId = match!.MatchId,
                TeamAId = match.TeamAId,
                TeamBId = match.TeamBId,
                Result = match.Result,
                Teams = teams
            };
        }
        // Guarda un partido (nuevo o editado) y recalcula el ranking.
        //Todo se ejecuta dentro de una transacción.
        public async Task SaveMatchAsync(RegisterMatchViewModel vm)
        {
            //Transacción
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                //Mismo equipo
                if (vm.TeamAId == vm.TeamBId)
                    throw new InvalidOperationException("Un equipo no puede jugar contra sí mismo.");

                var teamA = await _context.Teams
                    .Include(t => t.Ranking)
                    .FirstOrDefaultAsync(t => t.TeamId == vm.TeamAId);

                var teamB = await _context.Teams
                    .Include(t => t.Ranking)
                    .FirstOrDefaultAsync(t => t.TeamId == vm.TeamBId);

                if (teamA == null || teamB == null)
                    throw new InvalidOperationException("Equipo no encontrado.");

                //Niveles distintos
                if (teamA.Level != teamB.Level)
                    throw new InvalidOperationException("No se pueden registrar partidos entre niveles distintos.");

                Match match;

                // Crear nuevo partido
                if (vm.MatchId == 0)
                {
                    match = new Match
                    {
                        TeamAId = vm.TeamAId,
                        TeamBId = vm.TeamBId,
                        Result = vm.Result
                    };
                    _context.Matches.Add(match);
                }
                // Editar partido existente
                else
                {
                    match = await _context.Matches.FindAsync(vm.MatchId)
                        ?? throw new InvalidOperationException("Partido no encontrado.");

                    match.TeamAId = vm.TeamAId;
                    match.TeamBId = vm.TeamBId;
                    match.Result = vm.Result;
                }

                await _context.SaveChangesAsync();

                //Recalcular ranking de ambos equipos
                await RecalculateRankingForTeamsAsync(vm.TeamAId, vm.TeamBId);

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }


        // Elimina un partido y recalcula el ranking
        public async Task DeleteMatchAsync(int matchId)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var match = await _context.Matches.FindAsync(matchId);
                if (match == null) return;

                int teamAId = match.TeamAId;
                int teamBId = match.TeamBId;

                _context.Matches.Remove(match);
                await _context.SaveChangesAsync();

                // Recalcular ranking tras la eliminación
                await RecalculateRankingForTeamsAsync(teamAId, teamBId);

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }


        // Obtiene los últimos 20 partidos jugados.
        public async Task<List<MatchResultViewModel>> GetLastResultsAsync()
        {
            return await _context.Matches
                .Include(m => m.TeamA)
                .Include(m => m.TeamB)
                .OrderByDescending(m => m.PlayedAt)
                .Take(20)
                .Select(m => new MatchResultViewModel
                {
                    MatchId = m.MatchId,
                    TeamA = m.TeamA.Name!,
                    TeamALogoUrl = m.TeamA.LogoUrl,
                    TeamB = m.TeamB.Name!,
                    TeamBLogoUrl = m.TeamB.LogoUrl,
                    Result = m.Result,
                    Level = m.TeamA.Level,
                    PlayedAt = m.PlayedAt
                })
                .ToListAsync();
        }

        // Recalcula el ranking de dos equipos, Se utiliza tras crear, editar o eliminar un partido.
        public async Task RecalculateRankingForTeamsAsync(int teamAId, int teamBId)
        {
            await RecalculateTeamAsync(teamAId);
            await RecalculateTeamAsync(teamBId);
        }

        // Recalcula completamente el ranking de un equipo: puntos, último resultado y racha.
        private async Task RecalculateTeamAsync(int teamId)
        {
            var team = await _context.Teams
                .Include(t => t.Ranking)
                .FirstOrDefaultAsync(t => t.TeamId == teamId);

            if (team == null) return;

            var matches = await _context.Matches
                .Where(m => m.TeamAId == teamId || m.TeamBId == teamId)
                .OrderBy(m => m.PlayedAt)
                .ToListAsync();

            int points = 0;
            var streak = new List<char>();
            string? lastResult = null;

            foreach (var match in matches)
            {
                bool isTeamA = match.TeamAId == teamId;
                string result = match.Result;

                if (!isTeamA)
                {
                    result = result switch
                    {
                        "Victoria" => "Derrota",
                        "Derrota" => "Victoria",
                        _ => "Empate"
                    };
                }

                lastResult = result;
                //logica de puntos
                points += result switch
                {
                    "Victoria" => 3,
                    "Empate" => 1,
                    "Derrota" => -3,
                    _ => 0
                };
                // Guardar inicial del resultado para la racha
                streak.Add(result[0]);
            }

            team.Ranking ??= new Ranking { TeamId = teamId };

            team.Ranking.Points = points;
            team.Ranking.LastMatchResult = lastResult;
            team.Ranking.CurrentStreak = new string(streak.TakeLast(5).ToArray());

            await _context.SaveChangesAsync();
        }
    }
}

