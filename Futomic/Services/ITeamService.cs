using Futomic.Models;
using Futomic.View_Models;

namespace Futomic.Services
{
    public interface ITeamService
    {
        Task<List<Team>> GetTeamsAsync();
        Task<Team?> GetTeamDetailsAsync(int teamId);
        Task JoinTeamAsync(int teamId, string userId);
        Task CreateTeamAsync(TeamViewModel vm, IWebHostEnvironment env);
        Task<TeamEditViewModel?> GetTeamForEditAsync(int teamId);
        Task UpdateTeamAsync(TeamEditViewModel vm, IWebHostEnvironment env);
        Task DeleteTeamAsync(int teamId);
        Task<MyTeamViewModel?> GetMyTeamAsync(string userId);

    }
}
