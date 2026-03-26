using System.ComponentModel.DataAnnotations;
using System.IO;

namespace Futomic.Models
{
    public class Ranking
    {
        [Key]
        public int RankingId { get; set; }

        // Equipo asociado
        [Required]
        public int TeamId { get; set; }
        public Team? Team { get; set; }

        // Nivel del ranking (enum)
        [Required]
        public LevelTeam Level { get; set; }

        // Puntos acumulados
        public int Points { get; set; } = 0;

        // Último resultado del equipo
        public string? LastMatchResult { get; set; }
        public string CurrentStreak { get; set; } = "";

        
       
    }

    public enum ResultadoPartido
    {
        Victoria,
        Empate,
        Derrota
    }
}

