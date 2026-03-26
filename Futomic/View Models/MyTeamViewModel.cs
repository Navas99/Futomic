using Futomic.Models;

namespace Futomic.View_Models
{
    public class MyTeamViewModel
    {
        
        public int TeamId { get; set; }
        public string Name { get; set; } = "";
        public string? LogoUrl { get; set; }
        public LevelTeam Level { get; set; }
        public DateTime CreationDate { get; set; }

        
        public List<PlayerRowViewModel> Players { get; set; } = new();

        
        public List<TeamMatchViewModel> Matches { get; set; } = new();

        
        public TeamStatsViewModel Stats { get; set; } = new();
    }

}
