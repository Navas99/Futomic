using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Futomic.Models
{
    public enum EstadoReserva
    {
        Pendiente,
        Confirmada,
        Cancelada
    }

    public enum MetodoPago
    {
        Efectivo,
        Tarjeta,
        Bizum
    }
    public class Reservation
    {
        public int ReservationId { get; set; }
        [Required(ErrorMessage = "La fecha y hora es obligatorio")]
        public DateTime DateReservation { get; set; }
        
        public EstadoReserva State { get; set; } = EstadoReserva.Pendiente;
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        public MetodoPago Paymen { get; set; }
        public int Duration { get; set; }


        public Team? Team { get; set; }
        public int TeamId { get; set; }
        public Field? Field { get; set; }
        public int FieldId { get; set; }
    }
}
