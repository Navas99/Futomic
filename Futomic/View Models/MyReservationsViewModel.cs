using Futomic.Models;

namespace Futomic.View_Models
{
    public class MyReservationsViewModel
    {
        public int ReservationId { get; set; }
        public string FieldName { get; set; } = "";
        public string Location { get; set; } = "";
        public DateTime DateReservation { get; set; }
        public int Duration { get; set; }
        public decimal Price { get; set; }
        public EstadoReserva State { get; set; }
        public string TeamName { get; set; } = "";
    }
}
