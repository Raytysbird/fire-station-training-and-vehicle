using System;
using System.Collections.Generic;

namespace fire_station_training_and_vehicle.Models
{
    public partial class AssignedTask
    {
        public int TaskId { get; set; }
        public string UserId { get; set; } = null!;
        public bool? IsComplete { get; set; }

        public virtual UserTask Task { get; set; } = null!;
        public virtual AspNetUser User { get; set; } = null!;
    }
}
