using System.ComponentModel.DataAnnotations;

namespace Futomic.Models
{
    public enum LevelTeam { Principiante, Medio, Avanzado, Pro }
    public class Team
    {
        public int TeamId { get; set; }

        [Required(ErrorMessage = "El nombre del equipo es obligatorio")]
        [StringLength(50)]
        public string? Name { get; set; }

        [Required(ErrorMessage = "El capitán del equipo es obligatorio")]
        public string? Captain { get; set; }

        
        public string? LogoUrl { get; set; }      
       
        public DateTime CreationDate { get; set; } = DateTime.Now;
        public double AverageRating { get; set; } 

        
        public ICollection<User> Users { get; set; } = new List<User>();
        public ICollection<Field> Fields { get; set; } = new List<Field>();
        public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();

        
        public Ranking? Ranking { get; set; }


        public LevelTeam Level { get; set; }

        public ICollection<Match> MatchesAsTeamA { get; set; } = new List<Match>();
        public ICollection<Match> MatchesAsTeamB { get; set; } = new List<Match>();
    }
}
