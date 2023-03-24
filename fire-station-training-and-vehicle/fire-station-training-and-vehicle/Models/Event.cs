using System;
using System.Collections.Generic;

namespace fire_station_training_and_vehicle.Models
{
    public partial class Event
    {
        public Event()
        {
            AssignedEvents = new HashSet<AssignedEvent>();
        }

        public int EventId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Location { get; set; }
        public string? TeacherId { get; set; }
        public int? CourseId { get; set; }

        public virtual Course? Course { get; set; }
        public virtual ICollection<AssignedEvent> AssignedEvents { get; set; }
    }
}
