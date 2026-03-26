namespace Futomic.View_Models
{
    public class FieldAvailabilityViewModel
    {
        public int FieldId { get; set; }
        public string FieldName { get; set; } = "";
        public string Location { get; set; } = "";

        public DateTime SelectedDate { get; set; }

        public List<TimeSlotViewModel> TimeSlots { get; set; } = new();
    }
}
