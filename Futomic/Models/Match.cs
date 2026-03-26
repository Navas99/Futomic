using System.ComponentModel.DataAnnotations;

namespace Futomic.Models
{
    public class Match
    {
        public int MatchId { get; set; }

        [Required]
        public int TeamAId { get; set; }
        public Team TeamA { get; set; } = null!;

        [Required]
        public int TeamBId { get; set; }
        public Team TeamB { get; set; } = null!;

        
        public string Result { get; set; } = null!; // "victoria", "empate", "derrota" para TeamA

        public DateTime PlayedAt { get; set; } = DateTime.Now;
    }
}
