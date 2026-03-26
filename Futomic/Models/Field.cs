using System.ComponentModel.DataAnnotations;

namespace Futomic.Models
{
    public class Field
    {
        public int FieldId { get; set; }
        [Required(ErrorMessage = "El nombre del campo es obligatorio")]
        public string? Name { get; set; }
        [Required(ErrorMessage = "La localizacion del campo es obligatorio")]
        public string? Location { get; set; }
        [Required(ErrorMessage = "El email de contacto es obligatorio")]
        public string? EmailContact { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public string? PlusCode { get; set; }

        public ICollection<Team> Teams { get; set; } = new List<Team>();
        public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
    }
}
