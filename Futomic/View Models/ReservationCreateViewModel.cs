using Futomic.Models;
using System.ComponentModel.DataAnnotations;

namespace Futomic.View_Models
{
    public class ReservationCreateViewModel
    {
        public int FieldId { get; set; }

        [Required]
        [Display(Name = "Fecha y hora")]
        public DateTime DateReservation { get; set; }

        [Required]
        [Display(Name = "Duración")]
        public int Duration { get; set; }

        [Required]
        [Display(Name = "Método de pago")]
        public MetodoPago Paymen { get; set; }

        // Solo para mostrar info en la vista
        public string FieldName { get; set; } = "";
        public string FieldLocation { get; set; } = "";
        public decimal Price { get; set; }
    }

}
