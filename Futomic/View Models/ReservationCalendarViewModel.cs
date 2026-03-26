namespace Futomic.View_Models
{
    public class ReservationCalendarViewModel
    {
        public int FieldId { get; set; }
        public string FieldName { get; set; } = string.Empty;

        public DateTime SelectedDate { get; set; } = DateTime.Today;

        public List<TimeSlotViewModel> Slots { get; set; } = new();
    }
}
