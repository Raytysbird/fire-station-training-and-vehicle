namespace fire_station_training_and_vehicle.Models.Metadata
{
    public class CalendarEvent
    {
      
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string? Location { get; set; }
    }
}
