using System;
using System.Collections.Generic;

namespace fire_station_training_and_vehicle.Models
{
    public partial class AssignedTask
    {
        public int TaskId { get; set; }
        public string UserId { get; set; } = null!;
    }
}
