using Futomic.Models;

namespace Futomic.View_Models
{
    public class RegisterMatchViewModel
    {
        public int MatchId { get; set; } 
        public int TeamAId { get; set; }
        public int TeamBId { get; set; }

        public string Result { get; set; } = "Victoria";

        public IEnumerable<Team> Teams { get; set; } = new List<Team>();
    }
}
