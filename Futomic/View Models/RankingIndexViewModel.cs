using Futomic.Models;

namespace Futomic.View_Models
{
    public class RankingIndexViewModel
    {
        public LevelTeam? SelectedLevel { get; set; }
        public List<LevelTeam> Levels { get; set; } = new();
        public List<RankingRowViewModel> Rows { get; set; } = new();
    }
}
