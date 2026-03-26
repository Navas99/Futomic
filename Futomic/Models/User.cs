using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Futomic.Models
{
    public enum Posicion
    {
        Portero,
        Defensa,
        Medio,
        Delantero,
        Extremo
    }
    public class User : IdentityUser
    {
        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "El apellido es obligatorio")]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "El DNI es obligatorio")]
        [StringLength(9, ErrorMessage = "Máximo 9 caracteres")]
        public string? DNI { get; set; }

       
        public Posicion? Position { get; set; }

        public int? TeamId { get; set; }
        public Team? Team { get; set; }
    }
}
