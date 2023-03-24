using System;
using System.Collections.Generic;

namespace fire_station_training_and_vehicle.Models
{
    public partial class AssignedEvent
    {
        public int EventId { get; set; }
        public string UserId { get; set; } = null!;
        public bool? Attended { get; set; }
        public string? Grade { get; set; }

        public virtual Event Event { get; set; } = null!;
        public virtual AspNetUser User { get; set; } = null!;
    }
}
