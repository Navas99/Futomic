using Futomic.Models;
using System.ComponentModel.DataAnnotations;

namespace Futomic.View_Models
{
    public class TeamEditViewModel
    {
        public int TeamId { get; set; }

        [Required]
        public string Name { get; set; } = "";

        [Required]
        public string Captain { get; set; } = "";

        [Required]
        public LevelTeam Level { get; set; }

        public IFormFile? LogoFile { get; set; }

        public string? ExistingLogoUrl { get; set; }
    }

}
