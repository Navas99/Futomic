namespace Futomic.View_Models
{
    public class TeamStatsViewModel
    {
        public int MatchesPlayed { get; set; }
        public int Wins { get; set; }
        public int Draws { get; set; }
        public int Losses { get; set; }
        public int Points { get; set; }
        public string Form { get; set; } = ""; 
    }
}
