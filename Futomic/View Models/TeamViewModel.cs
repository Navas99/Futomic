using Futomic.Models;
using System.ComponentModel.DataAnnotations;

namespace Futomic.View_Models
{
    public class TeamViewModel
    {
        [Required(ErrorMessage = "El nombre del equipo es obligatorio")]
        public string? Name { get; set; }
        [Required(ErrorMessage = "El capitán es obligatorio")]
        public string? Captain { get; set; }
        public IFormFile? LogoFile { get; set; }


        [Required(ErrorMessage = "Selecciona un nivel")]
        public LevelTeam? Level { get; set; }

    }
}
