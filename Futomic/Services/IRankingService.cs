using Futomic.Models;
using Futomic.View_Models;

namespace Futomic.Services
{
    public interface IRankingService
    {
        Task<List<RankingRowViewModel>> GetRankingAsync(LevelTeam? level);
        Task<RegisterMatchViewModel> GetRegisterMatchAsync(int? matchId);
        Task SaveMatchAsync(RegisterMatchViewModel vm);
        Task DeleteMatchAsync(int matchId);
        Task<List<MatchResultViewModel>> GetLastResultsAsync();
        Task RecalculateRankingForTeamsAsync(int teamAId, int teamBId);
    }

}
