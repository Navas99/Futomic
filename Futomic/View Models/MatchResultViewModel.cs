using Futomic.Models;

namespace Futomic.View_Models
{
    public class MatchResultViewModel
    {
        public int MatchId { get; set; }
        public string? TeamALogoUrl { get; set; }
        public string? TeamBLogoUrl { get; set; }

        public string TeamA { get; set; } = "";
        public string TeamB { get; set; } = "";
        public string Result { get; set; } = "";
        public LevelTeam Level { get; set; }
        public DateTime PlayedAt { get; set; }
    }

}
