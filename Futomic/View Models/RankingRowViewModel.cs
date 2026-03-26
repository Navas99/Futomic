using Futomic.Models;

namespace Futomic.View_Models
{
    public class RankingRowViewModel
    {
        public int TeamId { get; set; }
        public string TeamName { get; set; } = "";
        public string? TeamLogoUrl { get; set; }
        public LevelTeam Level { get; set; }  
        public int Points { get; set; }
        public string LastMatchResult { get; set; } = "";
        public string CurrentStreak { get; set; } = "";
    }
}
